using System;
using Aspian.Domain.UserModel;

namespace Aspian.Domain.BaseModel
{
    public interface IEntityModify : IEntity, IEntityMetadata
    {
        DateTime? ModifiedAt { get; set; }
        string ModifiedById { get; set; }
        User ModifiedBy { get; set; }
    }
}