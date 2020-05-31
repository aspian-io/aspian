using System;
using System.Collections.Generic;
using Aspian.Domain.BaseModel;
using Aspian.Domain.PostModel;
using Aspian.Domain.SiteModel;

namespace Aspian.Domain.CommentModel
{
    public class Comment : Entitymeta, IComment
    {
        public string Content { get; set; }
        public bool Approved { get; set; }
        


        #region Navigation Properties
            public Guid? ParentId { get; set; }
            public virtual Comment Parent { get; set; }
            public virtual ICollection<Comment> Replies { get; set; }
            public Guid SiteId { get; set; }
            public virtual Site Site { get; set; }
            public Guid PostId { get; set; }
            public virtual Post Post { get; set; }
            public virtual ICollection<Commentmeta> Commentmetas { get; set; }
            public virtual ICollection<CommentHistory> CommentHistories { get; set; }
        #endregion
    }
}