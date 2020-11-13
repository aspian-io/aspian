using System.IO;

namespace Aspian.Application.Core.AttachmentServices.AdminServices.DTOs
{
    public class DownloadFileDto
    {
        public string FileName { get; set; }
        public Stream Stream { get; set; }
        public string MimeType { get; set; }
    }
}