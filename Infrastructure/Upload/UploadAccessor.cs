using System;
using System.IO;
using Aspian.Application.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Aspian.Domain.AttachmentModel;
using System.Collections.Generic;
using System.Linq;
using Aspian.Domain.OptionModel;
using Microsoft.Extensions.Options;
using FluentFTP;
using Aspian.Application.Core.AttachmentServices.AdminServices;

namespace Infrastructure.Upload
{
    public class UploadAccessor : IUploadAccessor
    {
        //
        private readonly IWebHostEnvironment _env;
        private readonly IUserAccessor _userAccessor;
        private readonly IOptionAccessor _optionAccessor;
        private readonly string _baseUri;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;
        public UploadAccessor(
            IWebHostEnvironment env,
            IUserAccessor userAccessor,
            IOptionAccessor optionAccessor,
            IOptions<FtpServerSettings> config
            )
        {
            _optionAccessor = optionAccessor;
            _userAccessor = userAccessor;
            _env = env;

            _baseUri = config.Value.ServerUri;
            _port = config.Value.ServerPort;
            _username = config.Value.ServerUsername;
            _password = config.Value.ServerPassword;
        }

        public async Task<FileUploadResult> AddFileAsync(IFormFile file, UploadLocationEnum uploadLocation)
        {
            var uploadFolderName = "uploads";
            var userUploadSubFolderName = _userAccessor.GetCurrentUsername();
            var userUploadSubFolderByDateName = DateTime.UtcNow.ToString("yyyy-MM-dd");
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{DateTime.UtcNow.ToString("H:mm:ss")}__{Path.GetRandomFileName()}{extension}";
            var mimeType = file.ContentType;
            var size = file.Length;
            var option = await _optionAccessor.GetOptionByKeyDescriptionAsync(mimeType);

            if (option.Value == ValueEnum.Attachments__NotAllowed)
                throw new Exception("You are not allowed to upload this type of file!");

            if (size > 0)
            {
                var fileType = CheckFileType(file);
                string fileRelativePath = null;

                if (uploadLocation == UploadLocationEnum.LocalHost)
                {
                    var root = _env.ContentRootPath;
                    Directory.CreateDirectory($"{_env.ContentRootPath}/{uploadFolderName}/{userUploadSubFolderName}/{userUploadSubFolderByDateName}/{fileType.ToString().ToLowerInvariant()}");
                    fileRelativePath = $"/{uploadFolderName}/{userUploadSubFolderName}/{userUploadSubFolderByDateName}/{fileType.ToString().ToLowerInvariant()}/{fileName}";
                    var fileAbsolutePath = $"{root}{fileRelativePath}";


                    using (var stream = File.Create(fileAbsolutePath))
                    {
                        if (IsFileTypeVerified(file))
                        {
                            await file.CopyToAsync(stream);
                        }
                        else
                        {
                            throw new Exception("Problem uploading the file!");
                        }
                    }
                }
                if (uploadLocation == UploadLocationEnum.FtpServer)
                {
                    var path = $"/{uploadFolderName}/{userUploadSubFolderName}/{userUploadSubFolderByDateName}/{fileType.ToString().ToLowerInvariant()}";
                    fileRelativePath = $"/{uploadFolderName}/{userUploadSubFolderName}/{userUploadSubFolderByDateName}/{fileType.ToString().ToLowerInvariant()}/{fileName}";
                    // create an FTP client
                    FtpClient client = new FtpClient(_baseUri, _port, _username, _password);
                    // begin connecting to the server
                    await client.ConnectAsync();
                    // check if a folder doesn't exist
                    if (!await client.DirectoryExistsAsync(path))
                        await client.CreateDirectoryAsync(path);

                    using (var stream = file.OpenReadStream())
                    {
                        if (IsFileTypeVerified(file))
                        {
                            // Upload file
                            var ftpStatus = await client.UploadAsync(stream, fileRelativePath);

                            if (ftpStatus.IsFailure())
                            {
                                await client.DisconnectAsync();
                                throw new Exception("Uploading file failed!");
                            }
                        }
                    }
                    // disconnect!
                    await client.DisconnectAsync();
                }

                return new FileUploadResult
                {
                    Type = fileType,
                    RelativePath = fileRelativePath,
                    MimeType = mimeType,
                    FileName = fileName,
                    FileExtension = extension,
                    FileSize = size,
                };
            }

            throw new Exception("Problem uploading the file!");
        }

        //
        public async Task<MemoryStream> GetImageAsync(string imageRelativePath, UploadLocationEnum uploadLocation)
        {
            if (uploadLocation == UploadLocationEnum.LocalHost)
            {
                var root = _env.ContentRootPath;
                var imageAbsolutePath = $"{root}{imageRelativePath}";
                if (File.Exists(imageAbsolutePath))
                {
                    var memory = new MemoryStream();
                    using (var stream = new FileStream(imageAbsolutePath, FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                    }

                    memory.Position = 0;
                    return memory;
                }
                throw new Exception("The requested image does not exist!");
            }

            if (uploadLocation == UploadLocationEnum.FtpServer)
            {
                // create an FTP client
                FtpClient client = new FtpClient(_baseUri, _port, _username, _password);
                // begin connecting to the server
                await client.ConnectAsync();
                if (await client.FileExistsAsync(imageRelativePath))
                {
                    var memory = new MemoryStream();
                    using (var stream = await client.OpenReadAsync(imageRelativePath))
                    {
                        await stream.CopyToAsync(memory);
                    }

                    memory.Position = 0;
                    // disconnect!
                    await client.DisconnectAsync();

                    return memory;
                }
                throw new Exception("The requested image does not exist!");
            }

            throw new Exception("Problem getting the requested image!");
        }

        //
        public async Task<MemoryStream> DownloadFileAsync(string fileRelativePath, UploadLocationEnum uploadLocation)
        {
            if (uploadLocation == UploadLocationEnum.LocalHost)
            {
                var root = _env.ContentRootPath;
                var fileAbsolutePath = $"{root}{fileRelativePath}";
                if (File.Exists(fileAbsolutePath))
                {
                    var memory = new MemoryStream();
                    using (var stream = new FileStream(fileAbsolutePath, FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                    }

                    memory.Position = 0;
                    return memory;
                }
                throw new Exception("The requested file does not exist!");
            }

            if (uploadLocation == UploadLocationEnum.FtpServer)
            {
                // create an FTP client
                FtpClient client = new FtpClient(_baseUri, _port, _username, _password);
                // begin connecting to the server
                await client.ConnectAsync();
                if (await client.FileExistsAsync(fileRelativePath))
                {
                    var memory = new MemoryStream();
                    using (var stream = await client.OpenReadAsync(fileRelativePath))
                    {
                        await stream.CopyToAsync(memory);
                    }

                    memory.Position = 0;
                    // disconnect!
                    await client.DisconnectAsync();

                    return memory;
                }
                throw new Exception("The requested file does not exist!");
            }

            throw new Exception("Problem downloading the requested file!");
        }

        //
        public async Task<string> DeleteFileAsync(string fileRelativePath, UploadLocationEnum uploadLocation)
        {
            if (uploadLocation == UploadLocationEnum.LocalHost)
            {
                var filePath = $"{_env.ContentRootPath}{fileRelativePath}";

                try
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                        return "ok";
                    }
                    else
                    {
                        throw new Exception("File not found!");
                    }
                }
                catch (IOException ioexception)
                {

                    throw new IOException(ioexception.Message);
                }
            }

            if (uploadLocation == UploadLocationEnum.FtpServer)
            {
                // create an FTP client
                FtpClient client = new FtpClient(_baseUri, _port, _username, _password);
                // begin connecting to the server
                await client.ConnectAsync();

                // check if a file exists
                if (await client.FileExistsAsync(fileRelativePath))
                {
                    // delete the file
                    await client.DeleteFileAsync(fileRelativePath);
                    return "ok";
                }
            }

            throw new Exception("Problem deleting the file!");
        }

        private AttachmentTypeEnum CheckFileType(IFormFile file)
        {
            var contentType = file.ContentType;

            var photoMimeTypes = new List<string>
            {
                "image/jpeg",
                "image/png",
                "image/svg+xml",
                "image/bmp",
                "image/gif"
            };

            var videoMimeTypes = new List<string>
            {
                "video/3gpp",
                "video/x-msvideo",
                "video/x-flv",
                "video/x-ms-wmv",
                "video/mpeg",
                "video/mp4",
                "video/x-matroska"
            };

            var audioMimeTypes = new List<string>
            {
                "audio/x-ms-wma",
                "audio/mpeg"
            };

            var pdfMimeType = "application/pdf";

            var textMimeTypes = new List<string> {
                "text/plain",
                "application/rtf",
                "application/msword",
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                "application/vnd.ms-excel",
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            };

            var compressedMimeTypes = new List<string> {
                "application/zip",
                "application/x-7z-compressed",
                "application/vnd.rar"
            };

            if (photoMimeTypes.Any(x => x.Equals(contentType)))
            {
                return AttachmentTypeEnum.Photo;
            }
            else if (videoMimeTypes.Any(x => x.Equals(contentType)))
            {
                return AttachmentTypeEnum.Video;
            }
            else if (audioMimeTypes.Any(x => x.Equals(contentType)))
            {
                return AttachmentTypeEnum.Audio;
            }
            else if (pdfMimeType.Equals(contentType))
            {
                return AttachmentTypeEnum.PDF;
            }
            else if (textMimeTypes.Any(x => x.Equals(contentType)))
            {
                return AttachmentTypeEnum.TextFile;
            }
            else if (compressedMimeTypes.Any(x => x.Equals(contentType)))
            {
                return AttachmentTypeEnum.Compressed;
            }
            else
            {
                return AttachmentTypeEnum.Other;
            }

            throw new Exception("Problem determining AttachmentType!");
        }


        private bool IsFileTypeVerified(IFormFile file)
        {
            // If you require a check on specific characters in the IsValidFileExtensionAndSignature
            // method, supply the characters in the _allowedChars field.
            byte[] _allowedChars = { };

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext))
            {
                return false;
            }

            // For more file signatures, see the File Signatures Database (https://www.filesignatures.net/)
            // and the official specifications for the file types you wish to add.
            Dictionary<string, List<byte[]>> _fileSignature = new Dictionary<string, List<byte[]>>
            {
                { ".gif", new List<byte[]> { new byte[] { 0x47, 0x49, 0x46, 0x38 } } },
                { ".bmp", new List<byte[]> { new byte[] { 0x42, 0x4D } } },
                { ".png", new List<byte[]> { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } } },
                { ".jpeg", new List<byte[]>
                    {
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                    }
                },
                { ".jpg", new List<byte[]>
                    {
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 },
                    }
                },

                { ".3gp", new List<byte[]>
                    {
                        new byte[] { 0x00, 0x00, 0x00, 0x14, 0x66, 0x74, 0x79, 0x70 },
                        new byte[] { 0x00, 0x00, 0x00, 0x20, 0x66, 0x74, 0x79, 0x70 },
                    }
                },
                { ".avi", new List<byte[]> { new byte[] { 0x52, 0x49, 0x46, 0x46 } } },
                { ".flv", new List<byte[]> { new byte[] { 0x46, 0x4C, 0x56 } } },
                { ".wmv", new List<byte[]> { new byte[] { 0x30, 0x26, 0xB2, 0x75, 0x8E, 0x66, 0xCF, 0x11 } } },
                { ".mpg", new List<byte[]>
                    {
                        new byte[] { 0x00, 0x00, 0x01, 0xBA },
                        new byte[] { 0x00, 0x00, 0x01, 0xB3 },
                    }
                },
                { ".mp4", new List<byte[]> { new byte[] { 0x00, 0x00, 0x00, 0x18, 0x66, 0x74, 0x79, 0x70, 0x6D, 0x70, 0x34, 0x32 } } },
                { ".mkv", new List<byte[]> { new byte[] { 0x1A, 0x45, 0xDF, 0xA3, 0x93, 0x42, 0x82, 0x88 } } },

                { ".wma", new List<byte[]> { new byte[] { 0x30, 0x26, 0xB2, 0x75, 0x8E, 0x66, 0xCF, 0x11 } } },
                { ".mp3", new List<byte[]> { new byte[] { 0x49, 0x44, 0x33 } } },

                { ".pdf", new List<byte[]> { new byte[] { 0x25, 0x50, 0x44, 0x46 } } },

                { ".rtf", new List<byte[]> { new byte[] { 0x7B, 0x5C, 0x72, 0x74, 0x66, 0x31 } } },
                { ".doc", new List<byte[]>
                    {
                        new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 },
                        new byte[] { 0x0D, 0x44, 0x4F, 0x43 },
                        new byte[] { 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1, 0x00 },
                        new byte[] { 0xDB, 0xA5, 0x2D, 0x00 },
                        new byte[] { 0xEC, 0xA5, 0xC1, 0x00 },
                    }
                },
                { ".docx", new List<byte[]>
                    {
                        new byte[] { 0x50, 0x4B, 0x03, 0x04 },
                        new byte[] { 0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x06, 0x00 },
                    }
                },
                { ".xls", new List<byte[]>
                    {
                        new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 },
                        new byte[] { 0x09, 0x08, 0x10, 0x00, 0x00, 0x06, 0x05, 0x00 },
                        new byte[] { 0xFD, 0xFF, 0xFF, 0xFF, 0x10 },
                        new byte[] { 0xFD, 0xFF, 0xFF, 0xFF, 0x1F },
                        new byte[] { 0xFD, 0xFF, 0xFF, 0xFF, 0x22 },
                        new byte[] { 0xFD, 0xFF, 0xFF, 0xFF, 0x23 },
                        new byte[] { 0xFD, 0xFF, 0xFF, 0xFF, 0x28 },
                        new byte[] { 0xFD, 0xFF, 0xFF, 0xFF, 0x29 },
                    }
                },
                { ".xlsx", new List<byte[]>
                    {
                        new byte[] { 0x50, 0x4B, 0x03, 0x04 },
                        new byte[] { 0x50, 0x4B, 0x03, 0x04, 0x14, 0x00, 0x06, 0x00 },
                    }
                },

                { ".zip", new List<byte[]>
                    {
                        new byte[] { 0x50, 0x4B, 0x03, 0x04 },
                        new byte[] { 0x50, 0x4B, 0x4C, 0x49, 0x54, 0x45 },
                        new byte[] { 0x50, 0x4B, 0x53, 0x70, 0x58 },
                        new byte[] { 0x50, 0x4B, 0x05, 0x06 },
                        new byte[] { 0x50, 0x4B, 0x07, 0x08 },
                        new byte[] { 0x57, 0x69, 0x6E, 0x5A, 0x69, 0x70 },
                    }
                },
                { ".rar", new List<byte[]> { new byte[] { 0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x00 } } },
                { ".7z", new List<byte[]> { new byte[] { 0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C } } },
            };
            using(var stream = file.OpenReadStream())
            using (var reader = new BinaryReader(stream))
            {
                if (ext.Equals(".txt") || ext.Equals(".csv") || ext.Equals(".prn") || ext.Equals(".svg"))
                {
                    if (_allowedChars.Length == 0)
                    {
                        // Limits characters to ASCII encoding.
                        for (var i = 0; i < stream.Length; i++)
                        {
                            if (reader.ReadByte() > sbyte.MaxValue)
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        // Limits characters to ASCII encoding and
                        // values of the _allowedChars array.
                        for (var i = 0; i < stream.Length; i++)
                        {
                            var b = reader.ReadByte();
                            if (b > sbyte.MaxValue ||
                                !_allowedChars.Contains(b))
                            {
                                return false;
                            }
                        }
                    }

                    return true;
                }

                // Uncomment the following code block if you must permit
                // files whose signature isn't provided in the _fileSignature
                // dictionary. We recommend that you add file signatures
                // for files (when possible) for all file types you intend
                // to allow on the system and perform the file signature
                // check.
                /*
                if (!_fileSignature.ContainsKey(ext))
                {
                    return true;
                }
                */

                // File signature check
                // --------------------
                // With the file signatures provided in the _fileSignature
                // dictionary, the following code tests the input content's
                // file signature.
                var signatures = _fileSignature[ext];
                var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

                return signatures.Any(signature =>
                    headerBytes.Take(signature.Length).SequenceEqual(signature));
            }
        }

    }
}