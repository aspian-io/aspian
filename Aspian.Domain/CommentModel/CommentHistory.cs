using System;
using Aspian.Domain.BaseModel;

namespace Aspian.Domain.CommentModel
{
    public class CommentHistory : Entitymeta, ICommentHistory
    {
        public string LastContent { get; set; }


        #region Navigaiotn Properties
            public Guid CommentId { get; set; }
            public virtual Comment Comment { get; set; }
        #endregion
    }
}