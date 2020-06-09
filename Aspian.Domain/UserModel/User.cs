using System.Collections.Generic;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.AttachmentModel;
using Aspian.Domain.CommentModel;
using Aspian.Domain.PostModel;
using Aspian.Domain.TaxonomyModel;
using Microsoft.AspNetCore.Identity;

namespace Aspian.Domain.UserModel
{
    public class User : IdentityUser, IUser
    {
        public string DisplayName { get; set; }
        public string Bio { get; set; }


        #region Navigation Properties
        public virtual ICollection<Usermeta> CreatedUsermetas { get; set; }
        public virtual ICollection<Usermeta> ModifiedUsermetas { get; set; }
        public virtual ICollection<Term> CreatedTerms { get; set; }
        public virtual ICollection<Term> ModifiedTerms { get; set; }
        public virtual ICollection<Termmeta> CreatedTermmetas { get; set; }
        public virtual ICollection<Termmeta> ModifiedTermmetas { get; set; }
        public virtual ICollection<TermTaxonomy> CreatedTermTaxonomies { get; set; }
        public virtual ICollection<TermTaxonomy> ModifiedTermTaxonomies { get; set; }
        public virtual ICollection<TermPost> CreatedTermPosts { get; set; }
        public virtual ICollection<TermPost> ModifiedTermPosts { get; set; }
        public virtual ICollection<Post> CreatedPosts { get; set; }
        public virtual ICollection<Post> ModifiedPosts { get; set; }
        public virtual ICollection<Postmeta> CreatedPostmetas { get; set; }
        public virtual ICollection<Postmeta> ModifiedPostmetas { get; set; }
        public virtual ICollection<Attachment> CreatedAttachments { get; set; }
        public virtual ICollection<Attachment> ModifiedAttachments { get; set; }
        public virtual ICollection<Attachmentmeta> CreatedAttachmentmetas { get; set; }
        public virtual ICollection<Attachmentmeta> ModifiedAttachmentmetas { get; set; }
        public virtual ICollection<PostHistory> CreatedPostHistories { get; set; }
        public virtual ICollection<PostHistory> ModifiedPostHistories { get; set; }
        public virtual ICollection<Comment> CreatedComments { get; set; }
        public virtual ICollection<Comment> ModifiedComments { get; set; }
        public virtual ICollection<Commentmeta> CreatedCommentmetas { get; set; }
        public virtual ICollection<Commentmeta> ModifiedCommentmetas { get; set; }
        public virtual ICollection<CommentHistory> CreatedCommentHistories { get; set; }
        public virtual ICollection<CommentHistory> ModifiedCommentHistories { get; set; }
        public virtual ICollection<ActivityOccurrence> ActivityOccurrences { get; set; }
        public virtual ICollection<ActivityOccurrencemeta> ActivityOccurrencemetas { get; set; }
        #endregion
    }
}