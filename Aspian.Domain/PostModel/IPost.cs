using System;
using System.Collections.Generic;
using Aspian.Domain.AttachmentModel;
using Aspian.Domain.BaseModel;
using Aspian.Domain.CommentModel;
using Aspian.Domain.SiteModel;
using Aspian.Domain.TaxonomyModel;

namespace Aspian.Domain.PostModel
{
    public interface IPost : IEntitymeta
    {
        string Title { get; set; }
        string Subtitle { get; set; }
        string Excerpt { get; set; }
        string Content { get; set; }
        string Slug { get; set; }
        PostVisibility Visibility { get; set; }
        PostStatusEnum PostStatus { get; set; }
        DateTime? ScheduledFor { get; set; }
        bool CommentAllowed { get; set; }
        int ViewCount { get; set; }
        PostTypeEnum Type { get; set; }
        bool IsPinned { get; set; }


        #region Navigation Properties
        ICollection<PostAttachment> PostAttachments { get; set; }
        Guid SiteId { get; set; }
        Site Site { get; set; }
        ICollection<TaxonomyPost> TaxonomyPosts { get; set; }
        ICollection<Postmeta> Postmetas { get; set; }
        ICollection<Comment> Comments { get; set; }
        #endregion
    }

    public enum PostStatusEnum
    {
        Publish,
        Future,
        Draft,
        Pending,
        AutoDraft,
        Inherit
    }

    public enum PostVisibility {
        Public,
        Private
    }

    public enum PostTypeEnum
    {
        Posts,
        Products,
        Pages
    }

    public enum PostMetaKeyEnum
    {

    }
}