using System;
using Aspian.Domain.UserModel;

namespace Aspian.Domain.BaseModel
{
    public interface IEntityCreate : IEntity, IEntityMetadata
    {
        DateTime CreatedDate { get; set; }
        string CreatedById { get; set; }
        User CreatedBy { get; set; }
    }
}