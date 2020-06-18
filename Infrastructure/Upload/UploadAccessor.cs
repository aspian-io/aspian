using System;
using System.IO;
using Aspian.Application.Core.Interfaces;
using Aspian.Application.Core.AttachmentServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Aspian.Domain.AttachmentModel;
using System.Collections.Generic;
using System.Linq;
using Aspian.Domain.OptionModel;
using System.Net;
using Microsoft.Extensions.Options;
using FluentFTP;

namespace Infrastructure.Upload
{
    public class UploadAccessor : IUploadAccessor
    {
        private readonly IWebHostEnvironment _env;
        private readonly IUserAccessor _userAccessor;
        private readonly IOptionAccessor _optionAccessor;
        private readonly string _baseUri;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;
        public UploadAccessor(IWebHostEnvironment env, IUserAccessor userAccessor, IOptionAccessor optionAccessor, IOptions<FtpServerSettings> config)
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
                        await file.CopyToAsync(stream);
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
                        // upload the file
                        var ftpStatus = await client.UploadAsync(stream, fileRelativePath);

                        if (ftpStatus.IsFailure())
                        {
                            await client.DisconnectAsync();
                            throw new Exception("Uploading file failed!");
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
                "image/tiff",
                "image/bmp",
                "image/gif"
            };

            var videoMimeTypes = new List<string>
            {
                "video/3gpp",
                "video/x-msvideo",
                "video/x-flv",
                "video/x-m4v",
                "video/x-ms-wm",
                "video/x-ms-wmv",
                "video/mpeg",
                "video/mp4",
                "video/ogg",
                "video/webm",
                "video/x-matroska"
            };

            var audioMimeTypes = new List<string>
            {
                "audio/x-aac",
                "audio/x-mpegurl",
                "audio/x-ms-wma",
                "audio/midi",
                "audio/mpeg",
                "audio/mp4",
                "audio/ogg",
                "audio/webm",
                "audio/x-wav",
                "audio/x-matroska"
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
            else
            {
                return AttachmentTypeEnum.Other;
            }

            throw new Exception("Problem determining AttachmentType!");
        }
    }
}