using System;
using System.Collections.Generic;
using Aspian.Domain.BaseModel;
using Aspian.Domain.PostModel;
using Aspian.Domain.SiteModel;
using Aspian.Domain.UserModel;

namespace Aspian.Domain.AttachmentModel
{
    public class Attachment : Entitymeta, IAttachment
    {
        public AttachmentTypeEnum Type { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FileSize { get; set; }
        public string MimeType { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }

        #region Navigation Properties
        public virtual ICollection<Attachmentmeta> Attachmentmetas { get; set; }

        public Guid? SiteId { get; set; }
        public virtual Site Site { get; set; }

        public Guid? AttachmentOwnerPostId { get; set; }
        public virtual Post AttachmentOwnerPost { get; set; }

        #endregion
    }
}