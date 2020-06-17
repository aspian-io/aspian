using Aspian.Domain.AttachmentModel;

namespace Aspian.Application.Core.AttachmentServices
{
    public class FileUploadResult
    {
        public AttachmentTypeEnum Type { get; set; }
        public string RelativePath { get; set; }
        public string MimeType { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public long FileSize { get; set; }
        public bool IsMain { get; set; }
        public string Result { get; set; }
    }
}