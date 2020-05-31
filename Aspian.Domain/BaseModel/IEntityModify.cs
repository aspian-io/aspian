using System;
using Aspian.Domain.UserModel;

namespace Aspian.Domain.BaseModel
{
    public interface IEntityModify : IEntity, IEntityMetadata
    {
        DateTime? ModifiedDate { get; set; }
        string ModifiedById { get; set; }
        User ModifiedBy { get; set; }
    }
}