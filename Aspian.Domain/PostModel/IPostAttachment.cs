using System;
using Aspian.Domain.AttachmentModel;
using Aspian.Domain.BaseModel;

namespace Aspian.Domain.PostModel
{
    public interface IPostAttachment : IEntityCreate, IEntityModify, IEntityInfo
    {
        bool IsMain { get; set; }
        Guid PostId { get; set; }
        Post Post { get; set; }
        Guid AttachmentId { get; set; }
        Attachment Attachment { get; set; }
    }
}