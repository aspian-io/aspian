using System;
using Aspian.Domain.AttachmentModel;

namespace Aspian.Application.Core.AttachmentServices.AdminServices.DTOs
{
    public class FileBrowserDto
    {
        public Guid Id { get; set; }
        public AttachmentTypeEnum Type { get; set; }
        public string MimeType { get; set; }
        public UploadLocationEnum UploadLocation { get; set; }
        public string RelativePath { get; set; }
        public string FileName { get; set; }
        public string PublicFileName { get; set; }
        public string FileExtension { get; set; }
        public long FileSize { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}