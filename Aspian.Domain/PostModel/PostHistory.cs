using System;
using Aspian.Domain.BaseModel;

namespace Aspian.Domain.PostModel
{
    public class PostHistory : Entitymeta, IPostHistory
    {
        public string Title { get; set; }
        public string  Subtitle { get; set; }
        public string Excerpt { get; set; }
        public string Content { get; set; }
        public string Slug { get; set; }
        public Guid? Parent { get; set; }
        public PostStatusEnum PostStatus { get; set; }
        public bool CommentAllowed { get; set; }
        public int Order { get; set; }


        #region Navigaiton Properties
            public Guid PostId { get; set; }
            public virtual Post Post { get; set; }
        #endregion
    }
}