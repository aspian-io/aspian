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

namespace Infrastructure.Upload
{
    public class UploadAccessor : IUploadAccessor
    {
        private readonly IWebHostEnvironment _env;
        private readonly IUserAccessor _userAccessor;
        private readonly IOptionAccessor _optionAccessor;
        public UploadAccessor(IWebHostEnvironment env, IUserAccessor userAccessor, IOptionAccessor optionAccessor)
        {
            _optionAccessor = optionAccessor;
            _userAccessor = userAccessor;
            _env = env;
        }

        public async Task<FileUploadResult> AddFileAsync(IFormFile file)
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
                Directory.CreateDirectory($"{_env.WebRootPath}/{uploadFolderName}/{userUploadSubFolderName}/{userUploadSubFolderByDateName}/{fileType.ToString().ToLowerInvariant()}");
                var filePath = $"{_env.WebRootPath}/{uploadFolderName}/{userUploadSubFolderName}/{userUploadSubFolderByDateName}/{fileType.ToString().ToLowerInvariant()}/{fileName}";
                using (var stream = File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }

                return new FileUploadResult
                {
                    Type = fileType,
                    Url = filePath,
                    MimeType = mimeType,
                    FileName = fileName,
                    FileExtension = extension,
                    FileSize = size
                };
            }

            throw new Exception("Problem uploading the file!");
        }

        public string DeleteFile(string filePath)
        {
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

        public AttachmentTypeEnum CheckFileType(IFormFile file)
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