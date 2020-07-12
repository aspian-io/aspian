using System;
using System.Collections.Generic;
using Aspian.Domain.BaseModel;
using Aspian.Domain.SiteModel;
using Aspian.Domain.UserModel;

namespace Aspian.Domain.ActivityModel
{
    public class ActivityOccurrence : EntityBase, IActivityOccurrence
    {
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedById { get; set; }
        public User CreatedBy { get; set; }
        public string UserAgent { get; set; }
        public string UserIPAddress { get; set; }
        

        #region Navigation Properties
            public Guid SiteId { get; set; }
            public virtual Site Site { get; set; }
            public int ActivityId { get; set; }
            public virtual Activity Activity { get; set; }
            public virtual ICollection<ActivityOccurrencemeta> Occurrencemetas { get; set; }
        #endregion
    }
}