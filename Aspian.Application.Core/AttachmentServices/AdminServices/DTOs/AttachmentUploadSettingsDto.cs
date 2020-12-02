using System.Collections.Generic;

namespace Aspian.Application.Core.AttachmentServices.AdminServices.DTOs
{
    public class AttachmentUploadSettingsDto
    {
        public bool IsMultipleUploadAllowed { get; set; }
        public bool IsAutoProceedUploadAllowed { get; set; }
        public int UploadMaxAllowedNumberOfFiles { get; set; }
        public int UploadMinAllowedNumberOfFiles { get; set; }
        public long UploadMaxAllowedFileSize { get; set; }
        public List<string> UploadAllowedFileTypes { get; set; }
    }
}