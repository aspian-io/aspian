using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Errors;
using Aspian.Application.Core.Interfaces;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.AttachmentModel;
using Aspian.Domain.SiteModel;
using Aspian.Domain.UserModel;
using Aspian.Persistence;
using FluentFTP;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using tusdotnet.Interfaces;

namespace Infrastructure.Upload
{
    public class TusUploadUtils : ITusUploadUtils
    {
        //
        private readonly IWebHostEnvironment _env;
        private readonly string _baseUri;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;
        private readonly bool _isActive;
        public TusUploadUtils(
            IWebHostEnvironment env,
            IOptions<FtpServerSettings> config,
            IServiceProvider services
            )
        {
            Services = services;
            _env = env;

            _baseUri = config.Value.ServerUri;
            _port = config.Value.ServerPort;
            _username = config.Value.ServerUsername;
            _password = config.Value.ServerPassword;
            _isActive = config.Value.IsActive;
        }

        private IServiceProvider Services { get; }
        private string UploadFolderName { get; set; }

        public string GetStorePath(string uploadForlderName,
            UploadLocationEnum uploadLocation = UploadLocationEnum.LocalHost,
            UploadLinkAccessibilityEnum linkAccessibility = UploadLinkAccessibilityEnum.Private)
        {
            UploadFolderName = uploadForlderName;
            using (var scope = Services.CreateScope())
            {
                var httpContextAccessor =
                scope.ServiceProvider
                    .GetRequiredService<IHttpContextAccessor>();

                var context =
                scope.ServiceProvider
                    .GetRequiredService<DataContext>();

                User user = null;

                var refreshToken = httpContextAccessor.HttpContext?.Request.Cookies["refreshToken"];

                if (refreshToken != null)
                {
                    var token = context.Tokens.SingleOrDefault(x => x.RefreshToken == refreshToken);
                    user = token != null ? token.CreatedBy : null;
                }

                var userUploadSubFolderName = user != null ? user.UserName : "unknown";

                if (linkAccessibility == UploadLinkAccessibilityEnum.Public)
                {
                    if (uploadLocation == UploadLocationEnum.LocalHost)
                    {
                        var localPublicStorePath = user != null ? 
                            Path.Combine("wwwroot", uploadForlderName, user.UserName) :
                            Path.Combine("wwwroot", uploadForlderName, "unknown");
                        return localPublicStorePath;
                    }

                    if (uploadLocation == UploadLocationEnum.FtpServer)
                    {
                        var ftpPublicStorePath = user != null ? 
                            Path.Combine("public_html", uploadForlderName, user.UserName) :
                            Path.Combine("public_html", uploadForlderName, "unknown");
                        return ftpPublicStorePath;
                    }
                        
                }

                var storePath = Path.Combine(uploadForlderName, userUploadSubFolderName);

                return storePath;
            }
        }

        public async Task SaveTusFileInfoAsync(ITusFile file, SiteTypeEnum siteType, string refreshToken, UploadLocationEnum location, CancellationToken cancellationToken)
        {
            var metadata = await file.GetMetadataAsync(cancellationToken);
            string name = metadata["name"].GetString(Encoding.UTF8);
            string type = metadata["type"].GetString(Encoding.UTF8);


            using (var scope = Services.CreateScope())
            {
                var httpContextAccessor =
                scope.ServiceProvider
                    .GetRequiredService<IHttpContextAccessor>();

                var context =
                scope.ServiceProvider
                    .GetRequiredService<DataContext>();

                var logger =
                scope.ServiceProvider
                    .GetRequiredService<IActivityLogger>();

                var token = context.Tokens.SingleOrDefault(x => x.RefreshToken == refreshToken);
                var user = token.CreatedBy;

                var storePath = Path.Combine(UploadFolderName, user.UserName);
                var extension = Path.GetExtension(name);
                var fileName = file.Id;
                var filePublicName = name;
                var mimeType = type;
                var fileRelativePath = Path.Combine(storePath, fileName);
                long size = 0;

                if (location == UploadLocationEnum.LocalHost)
                {
                    size = new FileInfo($"{storePath}/{fileName}").Length;
                }
                if (location == UploadLocationEnum.FtpServer)
                {
                    // create an FTP client
                    FtpClient client = new FtpClient(_baseUri, _port, _username, _password);
                    // begin connecting to the server
                    await client.ConnectAsync();

                    size = await client.GetFileSizeAsync($"{storePath}/{fileName}");

                    //
                    await client.DisconnectAsync();
                }


                var fileType = TusCheckFileType(type);

                var site = await context.Sites.SingleOrDefaultAsync(x => x.SiteType == siteType);

                user.CreatedAttachments.Add(new Attachment
                {
                    SiteId = site.Id,
                    UploadLocation = location,
                    Type = fileType,
                    RelativePath = fileRelativePath,
                    MimeType = mimeType,
                    FileName = fileName,
                    PublicFileName = filePublicName,
                    FileExtension = extension,
                    FileSize = size
                });

                var success = await context.SaveChangesAsync() > 0;

                if (success)
                {
                    await logger.LogActivity(
                        site.Id,
                        ActivityCodeEnum.AttachmentAdd,
                        ActivitySeverityEnum.Medium,
                        ActivityObjectEnum.Attachemnt,
                        $"The {fileType} file with the name {fileName} has been uploaded");
                }
                else
                {
                    throw new Exception("Problem saving file information in database!");
                }
            }

        }

        //
        public async Task DeleteTusFileAsync(string tusFileId, string fileRelativePath)
        {
            var filesToDeletePaths = new List<string>{
                fileRelativePath,
                $"{fileRelativePath}.chunkstart",
                $"{fileRelativePath}.chunkcomplete",
                $"{fileRelativePath}.metadata",
                $"{fileRelativePath}.uploadlength"
            };

            if (!_isActive)
            {
                foreach (var filePath in filesToDeletePaths)
                {
                    try
                    {
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
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
            }

            if (_isActive)
            {
                // create an FTP client
                FtpClient client = new FtpClient(_baseUri, _port, _username, _password);
                // begin connecting to the server
                await client.ConnectAsync();

                foreach (var filePath in filesToDeletePaths)
                {
                    // check if a file exists
                    if (await client.FileExistsAsync(filePath))
                    {
                        // delete the file
                        await client.DeleteFileAsync(filePath);
                    }
                    else
                    {
                        throw new Exception("File not found!");
                    }
                }
            }
        }

        private AttachmentTypeEnum TusCheckFileType(string type)
        {
            var contentType = type;

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

        private async Task<bool> TusIsFileTypeVerifiedAsync(ITusFile file, string fileName, CancellationToken cancellationToken)
        {
            // If you require a check on specific characters in the IsValidFileExtensionAndSignature
            // method, supply the characters in the _allowedChars field.
            byte[] _allowedChars = { };

            var ext = Path.GetExtension(fileName).ToLowerInvariant();
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
                { ".mp4", new List<byte[]>
                    {
                        new byte[] { 0x00, 0x00, 0x00, 0x14, 0x66, 0x74, 0x79, 0x70, 0x69, 0x73, 0x6F, 0x6D },
                        new byte[] { 0x00, 0x00, 0x00, 0x14, 0x66, 0x74, 0x79, 0x70, 0x4D, 0x53, 0x4E, 0x56 },
                        new byte[] { 0x00, 0x00, 0x00, 0x18, 0x66, 0x74, 0x79, 0x70, 0x69, 0x73, 0x6F, 0x6D },
                        new byte[] { 0x00, 0x00, 0x00, 0x18, 0x66, 0x74, 0x79, 0x70, 0x4D, 0x53, 0x4E, 0x56 },
                        new byte[] { 0x00, 0x00, 0x00, 0x20, 0x66, 0x74, 0x79, 0x70, 0x69, 0x73, 0x6F, 0x6D },
                        new byte[] { 0x00, 0x00, 0x00, 0x20, 0x66, 0x74, 0x79, 0x70, 0x4D, 0x53, 0x4E, 0x56 },
                    }
                },
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

            using (var stream = await file.GetContentAsync(cancellationToken))
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
                List<byte[]> signatures = _fileSignature.TryGetValue(ext, out signatures) ? _fileSignature[ext] : null;
                if (signatures == null)
                    return false;

                var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));
                return signatures.Any(signature =>
                    headerBytes.Take(signature.Length).SequenceEqual(signature));
            }
        }
    }
}