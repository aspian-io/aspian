using System;

namespace Aspian.Domain.BaseModel
{
    public interface IEntityBase : IEntity
    {
        Guid Id { get; set; }
    }
}