using System;
using Aspian.Domain.UserModel;

namespace Aspian.Domain.BaseModel
{
    public class EntityCreate : IEntityCreate
    {
        public DateTime CreatedAt { get; set; }
        public string CreatedById { get; set; }
        public User CreatedBy { get; set; }
        
    }
}