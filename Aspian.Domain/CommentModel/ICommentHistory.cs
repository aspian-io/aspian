using System;
using Aspian.Domain.BaseModel;

namespace Aspian.Domain.CommentModel
{
    public interface ICommentHistory : IEntityBase, IEntityCreate, IEntityInfo
    {
        string LastContent { get; set; }


        #region Navigaiotn Properties
            Guid CommentId { get; set; }
            Comment Comment { get; set; }
        #endregion
    }
}