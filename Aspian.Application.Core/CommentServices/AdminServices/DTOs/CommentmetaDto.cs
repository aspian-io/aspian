using System;

namespace Aspian.Application.Core.CommentServices.AdminServices.DTOs
{
    public class CommentmetaDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string CreatedById { get; set; }
        public virtual UserDto CreatedBy { get; set; }
        public string ModifiedById { get; set; }
        public virtual UserDto ModifiedBy { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        #region Navigation Properties
            public Guid CommentId { get; set; }
        #endregion
    }
}