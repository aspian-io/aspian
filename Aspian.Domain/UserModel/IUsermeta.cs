using System;
using Aspian.Domain.BaseModel;

namespace Aspian.Domain.UserModel
{
    public interface IUsermeta : IEntitymeta
    {
        string MetaKey { get; set; }
        string MetaValue { get; set; }
    }
}