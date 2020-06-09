namespace Aspian.Application.Core.AttachmentServices.DTOs
{
    public class AttachmentDto
    {
        public string Type { get; set; }
        public string Url { get; set; }
        public string MimeType { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public long FileSize { get; set; }
        public bool IsMain { get; set; }
    }
}