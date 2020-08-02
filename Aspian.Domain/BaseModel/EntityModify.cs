using System;
using Aspian.Domain.UserModel;

namespace Aspian.Domain.BaseModel
{
    public class EntityModify : IEntityModify
    {
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedById { get; set; }
        public virtual User ModifiedBy { get; set; }
    }
}