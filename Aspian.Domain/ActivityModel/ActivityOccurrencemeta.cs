using System;
using Aspian.Domain.BaseModel;
using Aspian.Domain.UserModel;

namespace Aspian.Domain.ActivityModel
{
    public class ActivityOccurrencemeta : EntityBase, IActivityOccurrencemeta
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedById { get; set; }
        public User CreatedBy { get; set; }
        public string UserAgent { get; set; }
        public string UserIPAddress { get; set; }
        

        #region Navigation Properties
            public Guid OccurenceId { get; set; }
            public ActivityOccurrence Occurrence { get; set; }
        #endregion
    }
}