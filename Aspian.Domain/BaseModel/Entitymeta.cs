using System;
using Aspian.Domain.UserModel;

namespace Aspian.Domain.BaseModel
{
    public class Entitymeta : IEntitymeta
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CreatedById { get; set; }
        public User CreatedBy { get; set; }
        public string ModifiedById { get; set; }
        public User ModifiedBy { get; set; }
        public string UserAgent { get; set; }
        public string UserIPAddress { get; set; }
    }
}