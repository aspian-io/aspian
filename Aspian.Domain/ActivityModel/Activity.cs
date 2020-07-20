using System;
using System.Collections.Generic;
using Aspian.Domain.BaseModel;
using Aspian.Domain.SiteModel;
using Aspian.Domain.UserModel;

namespace Aspian.Domain.ActivityModel
{
    public class Activity : IActivity
    {
        public Guid Id { get; set; }
        public ActivityCodeEnum Code { get; set; }
        public ActivitySeverityEnum Severity { get; set; }
        public ActivityObjectEnum ObjectName { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }
        public string UserAgent { get; set; }
        public string UserIPAddress { get; set; }


        #region Navigation Properties
        public Guid SiteId { get; set; }
        public virtual Site Site { get; set; }
        #endregion
    }


}