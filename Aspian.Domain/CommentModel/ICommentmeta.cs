using System;
using Aspian.Domain.BaseModel;

namespace Aspian.Domain.CommentModel
{
    public interface ICommentmeta : IEntitymeta
    {
        string Key { get; set; }
        string Value { get; set; }


        #region Navigation Properties
            Guid CommentId { get; set; }
            Comment Comment { get; set; }
        #endregion
    }
}