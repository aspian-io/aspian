using System;
using Aspian.Domain.AttachmentModel;
using Aspian.Domain.BaseModel;
using Aspian.Domain.UserModel;

namespace Aspian.Domain.PostModel
{
    public class PostAttachment : EntityInfo, IPostAttachment
    {
        public bool IsMain { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedById { get; set; }
        public virtual User ModifiedBy { get; set; }


        #region Navigation Properties
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }
        public Guid AttachmentId { get; set; }
        public virtual Attachment Attachment { get; set; }
        #endregion
    }
}