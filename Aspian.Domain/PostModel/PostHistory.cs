using System;
using Aspian.Domain.BaseModel;
using Aspian.Domain.UserModel;

namespace Aspian.Domain.PostModel
{
    public class PostHistory : EntityBase, IPostHistory
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Excerpt { get; set; }
        public string Content { get; set; }
        public string Slug { get; set; }
        public Guid? ParentId { get; set; }
        public PostStatusEnum PostStatus { get; set; }
        public bool CommentAllowed { get; set; }
        public int Order { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedById { get; set; }
        public User CreatedBy { get; set; }
        public string UserAgent { get; set; }
        public string UserIPAddress { get; set; }


        #region Navigaiton Properties
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }
        #endregion
    }
}