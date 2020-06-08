using System.Collections.Generic;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.AttachmentModel;
using Aspian.Domain.BaseModel;
using Aspian.Domain.CommentModel;
using Aspian.Domain.PostModel;
using Aspian.Domain.TaxonomyModel;

namespace Aspian.Domain.UserModel
{
    public interface IUser : IEntity
    {
        string DisplayName { get; set; }


        #region Navigation Properties
        ICollection<Attachment> Photos { get; set; }
        ICollection<Usermeta> CreatedUsermetas { get; set; }
        ICollection<Usermeta> ModifiedUsermetas { get; set; }
        ICollection<Term> CreatedTerms { get; set; }
        ICollection<Term> ModifiedTerms { get; set; }
        ICollection<Termmeta> CreatedTermmetas { get; set; }
        ICollection<Termmeta> ModifiedTermmetas { get; set; }
        ICollection<TermTaxonomy> CreatedTermTaxonomies { get; set; }
        ICollection<TermTaxonomy> ModifiedTermTaxonomies { get; set; }
        ICollection<TermPost> CreatedTermPosts { get; set; }
        ICollection<TermPost> ModifiedTermPosts { get; set; }
        ICollection<Post> CreatedPosts { get; set; }
        ICollection<Post> ModifiedPosts { get; set; }
        ICollection<Postmeta> CreatedPostmetas { get; set; }
        ICollection<Postmeta> ModifiedPostmetas { get; set; }
        ICollection<PostHistory> CreatedPostHistories { get; set; }
        ICollection<PostHistory> ModifiedPostHistories { get; set; }
        ICollection<Comment> CreatedComments { get; set; }
        ICollection<Comment> ModifiedComments { get; set; }
        ICollection<Commentmeta> CreatedCommentmetas { get; set; }
        ICollection<Commentmeta> ModifiedCommentmetas { get; set; }
        ICollection<CommentHistory> CreatedCommentHistories { get; set; }
        ICollection<CommentHistory> ModifiedCommentHistories { get; set; }
        ICollection<ActivityOccurrence> ActivityOccurrences { get; set; }
        ICollection<ActivityOccurrencemeta> ActivityOccurrencemetas { get; set; }
        #endregion
    }
}