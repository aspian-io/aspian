using System.Collections.Generic;

namespace Aspian.Domain.ActivityModel
{
    public class Activity : IActivity
    {
        public int Id { get; set; }
        public ActivitySeverityEnum Severity { get; set; }
        public string Object { get; set; }
        public string Message { get; set; }


        #region Navigation Properties
            public virtual ICollection<ActivityOccurrence> ActivityOccurrences { get; set; }
        #endregion
    }

    
}