using System.Collections.Generic;
using Aspian.Domain.ActivityModel;
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
            ICollection<Usermeta> CreatedUsermetas { get; set; }
            ICollection<Usermeta> ModifiedUsermetas { get; set; }
            ICollection<TermTaxonomy> CreatedTermTaxonomies { get; set; }
            ICollection<TermTaxonomy> ModifiedTermTaxonomies { get; set; }
            ICollection<Post> CreatedPosts { get; set; }
            ICollection<Post> ModifiedPosts { get; set; }
            ICollection<Comment> CreatedComments { get; set; }
            ICollection<Comment> ModifiedComments { get; set; }
            ICollection<ActivityOccurrence> ActivityOccurrences { get; set; }
        #endregion
    }
}