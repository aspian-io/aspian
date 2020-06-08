using System;
using Aspian.Domain.BaseModel;

namespace Aspian.Domain.AttachmentModel
{
    public class Attachmentmeta : Entitymeta, IAttachmentmeta
    {
        public string MetaKey { get; set; }
        public string MetaValue { get; set; }

        #region Navigation Properties
        public Guid PostId { get; set; }
        public virtual Attachment Attachment { get; set; }
        #endregion
    }
}