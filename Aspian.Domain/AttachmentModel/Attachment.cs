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
        public string MimeType { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }

        #region Navigation Properties
        public virtual ICollection<Attachmentmeta> Attachmentmetas { get; set; }

        public Guid? SiteId { get; set; }
        public virtual Site Site { get; set; }

        public Guid? PhotoOwnerPostId { get; set; }
        public virtual Post PhotoOwnerPost { get; set; }
        public Guid? VideoOwnerPostId { get; set; }
        public virtual Post VideoOwnerPost { get; set; }
        public Guid? AudioOwnerPostId { get; set; }
        public virtual Post AudioOwnerPost { get; set; }
        public Guid? PdfOwnerPostId { get; set; }
        public virtual Post PdfOwnerPost { get; set; }
        public Guid? TextFileOwnerPostId { get; set; }
        public virtual Post TextFileOwnerPost { get; set; }
        public Guid? OtherFileOwnerPostId { get; set; }
        public virtual Post OtherFileOwnerPost { get; set; }
        public Guid? AttachmentOwnerPostId { get; set; }
        public virtual Post AttachmentOwnerPost { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }
        #endregion
    }
}