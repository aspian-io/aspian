using System.IO;

namespace Aspian.Application.Core.AttachmentServices.DTOs
{
    public class GetImageDto
    {
        public string FileName { get; set; }
        public MemoryStream Memory { get; set; }
        public string MimeType { get; set; }
    }
}