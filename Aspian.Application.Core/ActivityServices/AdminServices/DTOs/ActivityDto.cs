using System;
using Aspian.Domain.ActivityModel;

namespace Aspian.Application.Core.ActivityServices.AdminServices.DTOs
{
    public class ActivityDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Severity { get; set; }
        public string ObjectName { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedById { get; set; }
        public virtual UserDto CreatedBy { get; set; }
        public string UserAgent { get; set; }
        public string UserIPAddress { get; set; }
        public virtual SiteDto Site { get; set; }
    }
}