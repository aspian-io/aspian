using System;
using Aspian.Domain.UserModel;

namespace Aspian.Domain.BaseModel
{
    public class Entitymeta : IEntitymeta
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }
        public string ModifiedById { get; set; }
        public virtual User ModifiedBy { get; set; }
        public string UserAgent { get; set; }
        public string UserIPAddress { get; set; }
    }
}