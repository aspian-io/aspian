using System;
using System.Collections.Generic;
using Aspian.Domain.CommentModel;

namespace Aspian.Application.Core.CommentServices.AdminServices.DTOs
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public bool Approved { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string CreatedById { get; set; }
        public virtual UserDto CreatedBy { get; set; }
        public string ModifiedById { get; set; }
        public virtual UserDto ModifiedBy { get; set; }
        public Guid? ParentId { get; set; }
        public virtual ICollection<CommentDto> Replies { get; set; }
        public Guid SiteId { get; set; }
        public Guid PostId { get; set; }
        public virtual ICollection<CommentmetaDto> Commentmetas { get; set; }
        //public virtual ICollection<CommentHistory> CommentHistories { get; set; }
    }
}