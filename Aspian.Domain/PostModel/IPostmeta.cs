using System;
using Aspian.Domain.BaseModel;

namespace Aspian.Domain.PostModel
{
    public interface IPostmeta : IEntitymeta
    {
        string MetaKey { get; set; }
        string MetaValue { get; set; }

        #region Navigation Properties
        Guid PostId { get; set; }
        Post Post { get; set; }
        #endregion
    }

    public enum PhotoUploadFieldsEnum
    {
        Id,
        Url,
        IsMain
    }
}