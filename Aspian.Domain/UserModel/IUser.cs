using System;
using System.Collections.Generic;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.AttachmentModel;
using Aspian.Domain.BaseModel;
using Aspian.Domain.CommentModel;
using Aspian.Domain.PostModel;
using Aspian.Domain.SiteModel;
using Aspian.Domain.TaxonomyModel;

namespace Aspian.Domain.UserModel
{
    public interface IUser : IEntity, IEntityModify, IEntityInfo
    {
        string DisplayName { get; set; }
        string Bio { get; set; }
        string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        DateTime? LastLoginDate { get; set; }


        #region Navigation Properties
        ICollection<Usermeta> CreatedUsermetas { get; set; }
        ICollection<Usermeta> ModifiedUsermetas { get; set; }
        ICollection<Site> ModifiedSites { get; set; }
        ICollection<Term> CreatedTerms { get; set; }
        ICollection<Term> ModifiedTerms { get; set; }
        ICollection<Termmeta> CreatedTermmetas { get; set; }
        ICollection<Termmeta> ModifiedTermmetas { get; set; }
        ICollection<Taxonomy> CreatedTaxonomies { get; set; }
        ICollection<Taxonomy> ModifiedTaxonomies { get; set; }
        ICollection<TaxonomyPost> CreatedTaxonomyPosts { get; set; }
        ICollection<TaxonomyPost> ModifiedTaxonomyPosts { get; set; }
        ICollection<Post> CreatedPosts { get; set; }
        ICollection<Post> ModifiedPosts { get; set; }
        ICollection<Postmeta> CreatedPostmetas { get; set; }
        ICollection<Postmeta> ModifiedPostmetas { get; set; }
        ICollection<PostHistory> CreatedPostHistories { get; set; }
        ICollection<PostAttachment> CreatedPostAttachments { get; set; }
        ICollection<PostAttachment> ModifiedPostAttachments { get; set; }
        ICollection<Attachment> CreatedAttachments { get; set; }
        ICollection<Attachment> ModifiedAttachments { get; set; }
        ICollection<Attachmentmeta> CreatedAttachmentmetas { get; set; }
        ICollection<Attachmentmeta> ModifiedAttachmentmetas { get; set; }
        ICollection<Comment> CreatedComments { get; set; }
        ICollection<Comment> ModifiedComments { get; set; }
        ICollection<Commentmeta> CreatedCommentmetas { get; set; }
        ICollection<Commentmeta> ModifiedCommentmetas { get; set; }
        ICollection<CommentHistory> CreatedCommentHistories { get; set; }
        ICollection<CommentHistory> ModifiedCommentHistories { get; set; }
        ICollection<Activity> Activities { get; set; }
        #endregion
    }
}