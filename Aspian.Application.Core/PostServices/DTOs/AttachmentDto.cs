using System;
using System.Collections.Generic;
using Aspian.Domain.AttachmentModel;

namespace Aspian.Application.Core.PostServices.DTOs
{
    public class AttachmentDto
    {
        public Guid Id { get; set; }
        public AttachmentTypeEnum Type { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FileSize { get; set; }
        public string MimeType { get; set; }
        public UploadLocationEnum UploadLocation { get; set; }
        public string RelativePath { get; set; }
        public bool IsMain { get; set; }

        #region Navigation Properties
        public virtual ICollection<AttachmentmetaDto> Attachmentmetas { get; set; }

        #endregion
    }
}