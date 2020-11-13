using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentFTP;
using Infrastructure.Upload.Tus.Extensions.Internal;
using Infrastructure.Upload.Tus.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using tusdotnet.Extensions;
using tusdotnet.Interfaces;
using tusdotnet.Models;
using tusdotnet.Models.Concatenation;
using tusdotnet.Stores;

namespace Infrastructure.Upload.Tus.Stores
{
    /// <summary>
    /// The built in data store that save files on disk.
    /// </summary>
    public class CustomTusDiskStore :
        ITusStore,
        ITusCreationStore,
        ITusReadableStore,
        ITusTerminationStore,
        ITusChecksumStore,
        ITusConcatenationStore,
        ITusExpirationStore,
        ITusCreationDeferLengthStore
    {
        //
        private readonly IOptions<FtpServerSettings> _config;
        //
        private readonly string _directoryPath;
        private readonly bool _deletePartialFilesOnConcat;
        private readonly InternalFileRep.FileRepFactory _fileRepFactory;

        // These are the read and write buffers, they will get the value of TusDiskBufferSize.Default if not set in the constructor.
        private readonly int _maxReadBufferSize;
        private readonly int _maxWriteBufferSize;

        // Use our own array pool to not leak data to other parts of the running app.
        private static readonly ArrayPool<byte> _bufferPool = ArrayPool<byte>.Create();

        /// <summary>
        /// Initializes a new instance of the <see cref="TusDiskStore"/> class.
        /// Using this overload will not delete partial files if a final concatenation is performed.
        /// </summary>
        /// <param name="directoryPath">The path on disk where to save files</param>
        public CustomTusDiskStore(string directoryPath, IOptions<FtpServerSettings> config) : this(directoryPath, false, TusDiskBufferSize.Default, config)
        {
            // Left blank.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TusDiskStore"/> class.
        /// </summary>
        /// <param name="directoryPath">The path on disk where to save files</param>
        /// <param name="deletePartialFilesOnConcat">True to delete partial files if a final concatenation is performed</param>

        public CustomTusDiskStore(string directoryPath, bool deletePartialFilesOnConcat, IOptions<FtpServerSettings> config) : this(directoryPath, deletePartialFilesOnConcat, TusDiskBufferSize.Default, config)
        {
            //_config = config;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TusDiskStore"/> class.
        /// </summary>
        /// <param name="directoryPath">The path on disk where to save files</param>
        /// <param name="deletePartialFilesOnConcat">True to delete partial files if a final concatenation is performed</param>
        /// <param name="bufferSize">The buffer size to use when reading and writing. If unsure use <see cref="TusDiskBufferSize.Default"/>.</param>
        public CustomTusDiskStore(string directoryPath, bool deletePartialFilesOnConcat, TusDiskBufferSize bufferSize, IOptions<FtpServerSettings> config)
        {
            _config = config;
            _directoryPath = directoryPath;
            _deletePartialFilesOnConcat = deletePartialFilesOnConcat;
            _fileRepFactory = new InternalFileRep.FileRepFactory(_directoryPath);

            if (bufferSize == null)
                bufferSize = TusDiskBufferSize.Default;

            _maxWriteBufferSize = bufferSize.WriteBufferSizeInBytes;
            _maxReadBufferSize = bufferSize.ReadBufferSizeInBytes;
        }

        /// <inheritdoc />
        public async Task<long> AppendDataAsync(string fileId, Stream stream, CancellationToken cancellationToken)
        {
            var client = new FtpClient(_config.Value.ServerUri, _config.Value.ServerPort, _config.Value.ServerUsername, _config.Value.ServerPassword);
            var internalFileId = new InternalFileId(fileId);

            var httpReadBuffer = _bufferPool.Rent(_maxReadBufferSize);
            var fileWriteBuffer = _bufferPool.Rent(Math.Max(_maxWriteBufferSize, _maxReadBufferSize));

            try
            {
                var fileUploadLengthProvidedDuringCreate = await GetUploadLengthAsync(fileId, cancellationToken);
                using var diskFileStream = await client.OpenAppendAsync(_fileRepFactory.Data(internalFileId, _config).Path);

                var totalDiskFileLength = diskFileStream.Length;
                if (fileUploadLengthProvidedDuringCreate == totalDiskFileLength)
                {
                    return 0;
                }

                var chunkCompleteFile = InitializeChunk(internalFileId, totalDiskFileLength);

                int numberOfbytesReadFromClient;
                var bytesWrittenThisRequest = 0L;
                var clientDisconnectedDuringRead = false;
                var writeBufferNextFreeIndex = 0;

                do
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    numberOfbytesReadFromClient = await stream.ReadAsync(httpReadBuffer, 0, _maxReadBufferSize, cancellationToken);
                    clientDisconnectedDuringRead = cancellationToken.IsCancellationRequested;

                    totalDiskFileLength += numberOfbytesReadFromClient;

                    if (totalDiskFileLength > fileUploadLengthProvidedDuringCreate)
                    {
                        throw new TusStoreException($"Stream contains more data than the file's upload length. Stream data: {totalDiskFileLength}, upload length: {fileUploadLengthProvidedDuringCreate}.");
                    }

                    // Can we fit the read data into the write buffer? If not flush it now.
                    if (writeBufferNextFreeIndex + numberOfbytesReadFromClient > _maxWriteBufferSize)
                    {
                        await FlushFileToDisk(internalFileId, fileWriteBuffer, diskFileStream, writeBufferNextFreeIndex);
                        writeBufferNextFreeIndex = 0;
                    }

                    Array.Copy(
                        sourceArray: httpReadBuffer,
                        sourceIndex: 0,
                        destinationArray: fileWriteBuffer,
                        destinationIndex: writeBufferNextFreeIndex,
                        length: numberOfbytesReadFromClient);

                    writeBufferNextFreeIndex += numberOfbytesReadFromClient;
                    bytesWrittenThisRequest += numberOfbytesReadFromClient;

                } while (numberOfbytesReadFromClient != 0);

                // Flush the remaining buffer to disk.
                if (writeBufferNextFreeIndex != 0)
                    await FlushFileToDisk(internalFileId, fileWriteBuffer, diskFileStream, writeBufferNextFreeIndex);

                if (!clientDisconnectedDuringRead)
                {
                    MarkChunkComplete(chunkCompleteFile);
                }

                return bytesWrittenThisRequest;
            }
            finally
            {
                _bufferPool.Return(httpReadBuffer);
                _bufferPool.Return(fileWriteBuffer);
            }
        }

        /// <inheritdoc />
        public Task<bool> FileExistAsync(string fileId, CancellationToken _)
        {
            return Task.FromResult(_fileRepFactory.Data(new InternalFileId(fileId), _config).Exist());
        }

        /// <inheritdoc />
        public Task<long?> GetUploadLengthAsync(string fileId, CancellationToken _)
        {
            var firstLine = _fileRepFactory.UploadLength(new InternalFileId(fileId), _config).ReadFirstLine(true);
            return Task.FromResult(firstLine == null
                ? (long?)null
                : long.Parse(firstLine));
        }

        /// <inheritdoc />
        public Task<long> GetUploadOffsetAsync(string fileId, CancellationToken _)
        {
            return Task.FromResult(_fileRepFactory.Data(new InternalFileId(fileId), _config).GetLength());
        }

        /// <inheritdoc />
        public async Task<string> CreateFileAsync(long uploadLength, string metadata, CancellationToken cancellationToken)
        {
            var fileId = new InternalFileId();
            var client = new FtpClient(_config.Value.ServerUri, _config.Value.ServerPort, _config.Value.ServerUsername, _config.Value.ServerPassword);
            client.OpenWrite(_fileRepFactory.Data(fileId, _config).Path).Dispose();

            if (uploadLength != -1)
            {
                await SetUploadLengthAsync(fileId.FileId, uploadLength, cancellationToken);
            }
            _fileRepFactory.Metadata(fileId, _config).Write(metadata);

            return fileId.FileId;
        }

        /// <inheritdoc />
        public Task<string> GetUploadMetadataAsync(string fileId, CancellationToken _)
        {
            var firstLine = _fileRepFactory.Metadata(new InternalFileId(fileId), _config).ReadFirstLine(true);
            return string.IsNullOrEmpty(firstLine) ? Task.FromResult<string>(null) : Task.FromResult(firstLine);
        }

        /// <inheritdoc />
        public Task<ITusFile> GetFileAsync(string fileId, CancellationToken _)
        {
            var internalFileId = new InternalFileId(fileId);
            var data = _fileRepFactory.Data(internalFileId, _config);

            return Task.FromResult<ITusFile>(data.Exist()
                ? new TusDiskFile(data, _fileRepFactory.Metadata(internalFileId, _config))
                : null);
        }

        /// <inheritdoc />
        public Task DeleteFileAsync(string fileId, CancellationToken _)
        {
            var internalFileId = new InternalFileId(fileId);
            return Task.Run(() =>
            {
                _fileRepFactory.Data(internalFileId, _config).Delete();
                _fileRepFactory.UploadLength(internalFileId, _config).Delete();
                _fileRepFactory.Metadata(internalFileId, _config).Delete();
                _fileRepFactory.UploadConcat(internalFileId, _config).Delete();
                _fileRepFactory.ChunkStartPosition(internalFileId, _config).Delete();
                _fileRepFactory.ChunkComplete(internalFileId, _config).Delete();
                _fileRepFactory.Expiration(internalFileId, _config).Delete();
            });
        }

        /// <inheritdoc />
        public Task<IEnumerable<string>> GetSupportedAlgorithmsAsync(CancellationToken _)
        {
            return Task.FromResult<IEnumerable<string>>(new[] { "sha1" });
        }

        /// <inheritdoc />
        public Task<bool> VerifyChecksumAsync(string fileId, string algorithm, byte[] checksum, CancellationToken _)
        {
            var valid = false;
            var internalFileId = new InternalFileId(fileId);
            using (var dataStream = _fileRepFactory.Data(internalFileId, _config).GetStream(FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
            {
                var chunkPositionFile = _fileRepFactory.ChunkStartPosition(internalFileId, _config);
                var chunkStartPosition = chunkPositionFile.ReadFirstLineAsLong(true, 0);
                var chunkCompleteFile = _fileRepFactory.ChunkComplete(internalFileId, _config);

                // Only verify the checksum if the entire lastest chunk has been written.
                // If not, just discard the last chunk as it won't match the checksum anyway.
                if (chunkCompleteFile.Exist())
                {
                    var calculateSha1 = dataStream.CalculateSha1(chunkStartPosition);
                    valid = checksum.SequenceEqual(calculateSha1);
                }

                if (!valid)
                {
                    dataStream.Seek(0, SeekOrigin.Begin);
                    dataStream.SetLength(chunkStartPosition);
                }
            }

            return Task.FromResult(valid);
        }

        /// <inheritdoc />
        public Task<FileConcat> GetUploadConcatAsync(string fileId, CancellationToken _)
        {
            var firstLine = _fileRepFactory.UploadConcat(new InternalFileId(fileId), _config).ReadFirstLine(true);
            return Task.FromResult(string.IsNullOrWhiteSpace(firstLine)
                ? null
                : new UploadConcat(firstLine).Type);
        }

        /// <inheritdoc />
        public async Task<string> CreatePartialFileAsync(long uploadLength, string metadata, CancellationToken cancellationToken)
        {
            var fileId = await CreateFileAsync(uploadLength, metadata, cancellationToken);
            _fileRepFactory.UploadConcat(new InternalFileId(fileId), _config).Write(new FileConcatPartial().GetHeader());
            return fileId;
        }

        /// <inheritdoc />
        public async Task<string> CreateFinalFileAsync(string[] partialFiles, string metadata, CancellationToken cancellationToken)
        {
            var partialInternalFileReps = partialFiles.Select(f =>
            {
                var partialData = _fileRepFactory.Data(new InternalFileId(f), _config);

                if (!partialData.Exist())
                {
                    throw new TusStoreException($"File {f} does not exist");
                }

                return partialData;
            }).ToArray();

            var length = partialInternalFileReps.Sum(f => f.GetLength());

            var fileId = await CreateFileAsync(length, metadata, cancellationToken);

            var internalFileId = new InternalFileId(fileId);

            _fileRepFactory.UploadConcat(internalFileId, _config).Write(new FileConcatFinal(partialFiles).GetHeader());

            using (var finalFile = _fileRepFactory.Data(internalFileId, _config).GetStream(FileMode.Open, FileAccess.Write, FileShare.None))
            {
                foreach (var partialFile in partialInternalFileReps)
                {
                    using var partialStream = partialFile.GetStream(FileMode.Open, FileAccess.Read, FileShare.Read);
                    await partialStream.CopyToAsync(finalFile);
                }
            }

            if (_deletePartialFilesOnConcat)
            {
                await Task.WhenAll(partialInternalFileReps.Select(f => DeleteFileAsync(f.FileId, cancellationToken)));
            }

            return fileId;
        }

        /// <inheritdoc />
        public Task SetExpirationAsync(string fileId, DateTimeOffset expires, CancellationToken _)
        {
            _fileRepFactory.Expiration(new InternalFileId(fileId), _config).Write(expires.ToString("O"));
            return TaskHelper.Completed;
        }

        /// <inheritdoc />
        public Task<DateTimeOffset?> GetExpirationAsync(string fileId, CancellationToken _)
        {
            var expiration = _fileRepFactory.Expiration(new InternalFileId(fileId), _config).ReadFirstLine(true);

            return Task.FromResult(expiration == null
                ? (DateTimeOffset?)null
                : DateTimeOffset.ParseExact(expiration, "O", null));
        }

        /// <inheritdoc />
        public Task<IEnumerable<string>> GetExpiredFilesAsync(CancellationToken _)
        {
            var expiredFiles = Directory.EnumerateFiles(_directoryPath, "*.expiration")
                .Select(Path.GetFileNameWithoutExtension)
                .Select(f => new InternalFileId(f))
                .Where(f => FileHasExpired(f) && FileIsIncomplete(f))
                .Select(f => f.FileId)
                .ToList();

            return Task.FromResult<IEnumerable<string>>(expiredFiles);

            bool FileHasExpired(InternalFileId fileId)
            {
                var firstLine = _fileRepFactory.Expiration(fileId, _config).ReadFirstLine();
                return !string.IsNullOrWhiteSpace(firstLine)
                       && DateTimeOffset.ParseExact(firstLine, "O", null).HasPassed();
            }

            bool FileIsIncomplete(InternalFileId fileId)
            {
                var uploadLength = _fileRepFactory.UploadLength(fileId, _config).ReadFirstLineAsLong(fileIsOptional: true, defaultValue: long.MinValue);

                if (uploadLength == long.MinValue)
                {
                    return true;
                }

                var dataFile = _fileRepFactory.Data(fileId, _config);

                if (!dataFile.Exist())
                {
                    return true;
                }

                return uploadLength != dataFile.GetLength();
            }
        }

        /// <inheritdoc />
        public async Task<int> RemoveExpiredFilesAsync(CancellationToken cancellationToken)
        {
            var expiredFiles = await GetExpiredFilesAsync(cancellationToken);
            var deleteFileTasks = expiredFiles.Select(file => DeleteFileAsync(file, cancellationToken)).ToList();

            await Task.WhenAll(deleteFileTasks);

            return deleteFileTasks.Count;
        }

        /// <inheritdoc />
        public Task SetUploadLengthAsync(string fileId, long uploadLength, CancellationToken _)
        {
            _fileRepFactory.UploadLength(new InternalFileId(fileId), _config).Write(uploadLength.ToString());
            return TaskHelper.Completed;
        }

        private InternalFileRep InitializeChunk(InternalFileId internalFileId, long totalDiskFileLength)
        {
            var chunkComplete = _fileRepFactory.ChunkComplete(internalFileId, _config);
            chunkComplete.Delete();
            _fileRepFactory.ChunkStartPosition(internalFileId, _config).Write(totalDiskFileLength.ToString());

            return chunkComplete;
        }

        private void MarkChunkComplete(InternalFileRep chunkComplete)
        {
            chunkComplete.Write("1");
        }

        private async Task FlushFileToDisk(InternalFileId internalFileId, byte[] fileWriteBuffer, Stream stream, int writeBufferNextFreeIndex)
        {
                var path = _fileRepFactory.Data(internalFileId, _config).Path;

                var client = new FtpClient(_config.Value.ServerUri, _config.Value.ServerPort, _config.Value.ServerUsername, _config.Value.ServerPassword);

                await stream.WriteAsync(fileWriteBuffer, 0, writeBufferNextFreeIndex);
                await stream.FlushAsync();
        }
    }
}