using System;
using Aspian.Domain.BaseModel;

namespace Aspian.Domain.PostModel
{
    public interface IPostmeta : IEntitymeta
    {
        PostMetaKeyEnum MetaKey { get; set; }
        string MetaValue { get; set; }

        #region Navigation Properties
        Guid PostId { get; set; }
        Post Post { get; set; }
        #endregion
    }
}