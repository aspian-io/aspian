using System;
using Aspian.Domain.BaseModel;

namespace Aspian.Domain.CommentModel
{
    public class Commentmeta : Entitymeta, ICommentmeta
    {
        public string Key { get; set; }
        public string Value { get; set; }

        #region Navigation Properties
            public Guid CommentId { get; set; }
            public virtual Comment Comment { get; set; }
        #endregion
    }
}