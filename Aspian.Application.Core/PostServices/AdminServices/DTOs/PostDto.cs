using System;
using System.Collections.Generic;
using Aspian.Domain.AttachmentModel;
using Aspian.Domain.PostModel;
using Aspian.Domain.TaxonomyModel;
using Aspian.Domain.UserModel;

namespace Aspian.Application.Core.PostServices.AdminServices.DTOs
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Excerpt { get; set; }
        public string Content { get; set; }
        public string Slug { get; set; }
        public PostStatusEnum PostStatus { get; set; }
        public bool CommentAllowed { get; set; }
        public int Order { get; set; }
        public int ViewCount { get; set; }
        public PostTypeEnum Type { get; set; }
        public bool IsPinned { get; set; }
        public int PinOrder { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedById { get; set; }


        #region Navigation Properties
        public virtual UserDto CreatedBy { get; set; }
        public virtual ICollection<PostAttachmentDto> PostAttachments { get; set; }
        public Guid? ParentId { get; set; }
        public virtual PostDto Parent { get; set; }
        public virtual ICollection<PostDto> ChildPosts { get; set; }
        // public Guid SiteId { get; set; }
        // public virtual Site Site { get; set; }
        public virtual ICollection<TaxonomyPostDto> TaxonomyPosts { get; set; }
        public virtual ICollection<PostmetaDto> Postmetas { get; set; }
        // public virtual ICollection<PostHistory> PostHistories { get; set; }
        // public virtual ICollection<Comment> Comments { get; set; }
        #endregion
    }
}