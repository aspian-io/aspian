namespace Aspian.Application.Core.UserServices.UserServices.DTOs
{
    public class PhotoDto
    {
        public string Type { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FileSize { get; set; }
        public string MimeType { get; set; }
        public string UploadLocation { get; set; }
        public string RelativePath { get; set; }
        public bool IsMain { get; set; }
    }
}