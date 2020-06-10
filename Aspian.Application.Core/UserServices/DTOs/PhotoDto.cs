namespace Aspian.Application.Core.UserServices.DTOs
{
    public class PhotoDto
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