using System;
using Aspian.Domain.BaseModel;

namespace Aspian.Domain.AttachmentModel
{
    public interface IAttachmentmeta : IEntitymeta
    {
        string MetaKey { get; set; }
        string MetaValue { get; set; }

        #region Navigation Properties
        Guid PostId { get; set; }
        Attachment Attachment { get; set; }
        #endregion
    }
}