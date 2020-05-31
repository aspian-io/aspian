using System;
using Aspian.Domain.UserModel;

namespace Aspian.Domain.BaseModel
{
    public class EntityModify : IEntityModify
    {
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedById { get; set; }
        public User ModifiedBy { get; set; }
    }
}