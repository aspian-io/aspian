using System.Collections.Generic;
using Aspian.Domain.BaseModel;

namespace Aspian.Domain.ActivityModel
{
    public interface IActivity
    {
        int Id { get; set; }
        ActivitySeverityEnum Severity { get; set; }
        string Object { get; set; }
        string Message { get; set; }


        #region Navigation Properties
            ICollection<ActivityOccurrence> ActivityOccurrences { get; set; }
        #endregion
    }

    public enum ActivitySeverityEnum
    {
        Information,
        Low,
        Medium,
        High,
        Critical
    }
}