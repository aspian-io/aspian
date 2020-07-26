using System;

namespace Aspian.Application.Core.PostServices.AdminServices.DTOs
{
    public class PostAttachmentDto
    {
        public bool IsMain { get; set; }
        public Guid AttachmentId { get; set; }
        public virtual AttachmentDto Attachment { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedById { get; set; }
    }
}