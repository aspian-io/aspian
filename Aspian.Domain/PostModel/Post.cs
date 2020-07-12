using System;
using System.Collections.Generic;
using Aspian.Domain.AttachmentModel;
using Aspian.Domain.BaseModel;
using Aspian.Domain.CommentModel;
using Aspian.Domain.SiteModel;
using Aspian.Domain.TaxonomyModel;

namespace Aspian.Domain.PostModel
{
    public class Post : Entitymeta, IPost
    {
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


        #region Navigation Properties
        public virtual ICollection<PostAttachment> PostAttachments { get; set; }
        public Guid? ParentId { get; set; }
        public virtual Post Parent { get; set; }
        public virtual ICollection<Post> ChildPosts { get; set; }
        public Guid SiteId { get; set; }
        public virtual Site Site { get; set; }
        public virtual ICollection<TaxonomyPost> TaxonomyPosts { get; set; }
        public virtual ICollection<Postmeta> Postmetas { get; set; }
        public virtual ICollection<PostHistory> PostHistories { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        #endregion

    }


}
