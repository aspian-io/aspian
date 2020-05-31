using System;
using Aspian.Domain.BaseModel;

namespace Aspian.Domain.CommentModel
{
    public interface ICommentHistory : IEntitymeta
    {
        string LastContent { get; set; }


        #region Navigaiotn Properties
            Guid CommentId { get; set; }
            Comment Comment { get; set; }
        #endregion
    }
}