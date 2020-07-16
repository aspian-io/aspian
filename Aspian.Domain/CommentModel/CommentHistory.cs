using System;
using Aspian.Domain.BaseModel;
using Aspian.Domain.UserModel;

namespace Aspian.Domain.CommentModel
{
    public class CommentHistory : EntityBase, ICommentHistory
    {
        public string LastContent { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedById { get; set; }
        public User CreatedBy { get; set; }
        public string UserAgent { get; set; }
        public string UserIPAddress { get; set; }


        #region Navigaiotn Properties
            public Guid CommentId { get; set; }
            public virtual Comment Comment { get; set; }
        #endregion
    }
}