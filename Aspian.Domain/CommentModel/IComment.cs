using System;
using System.Collections.Generic;
using Aspian.Domain.BaseModel;
using Aspian.Domain.PostModel;
using Aspian.Domain.SiteModel;

namespace Aspian.Domain.CommentModel
{
    public interface IComment : IEntitymeta
    {
        string Content { get; set; }
        bool Approved { get; set; }


        #region Navigation Properties
            Guid? ParentId { get; set; }
            Comment Parent { get; set; }
            ICollection<Comment> Replies { get; set; }
            Guid SiteId { get; set; }
            Site Site { get; set; }
            Guid PostId { get; set; }
            Post Post { get; set; }
            ICollection<Commentmeta> Commentmetas { get; set; }
            ICollection<CommentHistory> CommentHistories { get; set; }
        #endregion
    }
}