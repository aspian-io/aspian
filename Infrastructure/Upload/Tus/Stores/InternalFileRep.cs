using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentFTP;
using Microsoft.Extensions.Options;

namespace Infrastructure.Upload.Tus.Stores
{
    internal sealed class InternalFileRep
    {
        public string Path { get; }

        public string FileId { get; set; }

        private readonly IOptions<FtpServerSettings> _config;

        private InternalFileRep(string fileId, string path, IOptions<FtpServerSettings> config)
        {
            FileId = fileId;
            Path = path;

            _config = config;
        }



        public void Delete()
        {
            var client = new FtpClient(_config.Value.ServerUri, _config.Value.ServerPort, _config.Value.ServerUsername, _config.Value.ServerPassword);
            if (client.FileExists(Path))
                client.DeleteFile(Path);
        }

        public bool Exist()
        {
            var client = new FtpClient(_config.Value.ServerUri, _config.Value.ServerPort, _config.Value.ServerUsername, _config.Value.ServerPassword);
            var fileExists = client.FileExists(Path);

            return fileExists;
        }

        public void Write(string text)
        {
            var client = new FtpClient(_config.Value.ServerUri, _config.Value.ServerPort, _config.Value.ServerUsername, _config.Value.ServerPassword);
            using (var stream = client.OpenWrite(Path, FtpDataType.ASCII, true))
            {
                var sr = new StreamWriter(stream, Encoding.UTF8);
                sr.WriteLine(text);
                sr.Flush();
            }
        }

        public long ReadFirstLineAsLong(bool fileIsOptional = false, long defaultValue = -1)
        {
            var content = ReadFirstLine(fileIsOptional);
            if (long.TryParse(content, out var value))
            {
                return value;
            }

            return defaultValue;
        }

        public string ReadFirstLine(bool fileIsOptional = false)
        {
            var client = new FtpClient(_config.Value.ServerUri, _config.Value.ServerPort, _config.Value.ServerUsername, _config.Value.ServerPassword);
            if (fileIsOptional && !client.FileExists(Path))
            {
                return null;
            }

            using var stream = client.OpenRead(Path);
            using var sr = new StreamReader(stream);
            var readSr = sr.ReadLine();
            return readSr;
        }

        public Stream GetStream(FileMode mode, FileAccess access, FileShare share)
        {
            var client = new FtpClient(_config.Value.ServerUri, _config.Value.ServerPort, _config.Value.ServerUsername, _config.Value.ServerPassword);
            using var file = mode == FileMode.Append ? client.OpenAppend(Path) : client.OpenWrite(Path);

            return file;
        }

        public long GetLength()
        {
            if (_config.Value.IsActive)
            {
                var client = new FtpClient(_config.Value.ServerUri, _config.Value.ServerPort, _config.Value.ServerUsername, _config.Value.ServerPassword);
                var fileSize = client.GetFileSize(Path);

                return fileSize;
            }
            else
            {
                return new FileInfo(Path).Length;
            }
        }

        internal sealed class FileRepFactory
        {
            private readonly string _directoryPath;

            public FileRepFactory(string directoryPath)
            {
                _directoryPath = directoryPath;
            }

            public InternalFileRep Data(InternalFileId fileId, IOptions<FtpServerSettings> config) => Create(fileId, "", config);

            public InternalFileRep UploadLength(InternalFileId fileId, IOptions<FtpServerSettings> config) => Create(fileId, "uploadlength", config);

            public InternalFileRep UploadConcat(InternalFileId fileId, IOptions<FtpServerSettings> config) => Create(fileId, "uploadconcat", config);

            public InternalFileRep Metadata(InternalFileId fileId, IOptions<FtpServerSettings> config) => Create(fileId, "metadata", config);

            public InternalFileRep Expiration(InternalFileId fileId, IOptions<FtpServerSettings> config) => Create(fileId, "expiration", config);

            public InternalFileRep ChunkStartPosition(InternalFileId fileId, IOptions<FtpServerSettings> config) => Create(fileId, "chunkstart", config);

            public InternalFileRep ChunkComplete(InternalFileId fileId, IOptions<FtpServerSettings> config) => Create(fileId, "chunkcomplete", config);

            private InternalFileRep Create(InternalFileId fileId, string extension, IOptions<FtpServerSettings> config)
            {
                var fileName = fileId.FileId;
                if (!string.IsNullOrEmpty(extension))
                {
                    fileName += "." + extension;
                }

                return new InternalFileRep(fileId.FileId, System.IO.Path.Combine(_directoryPath, fileName), config);
            }
        }
    }
}