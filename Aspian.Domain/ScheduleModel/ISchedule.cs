using System;
using Aspian.Domain.BaseModel;

namespace Aspian.Domain.ScheduleModel
{
    public interface ISchedule : IEntityBase
    {
        ScheduleTypeEnum ScheduleType { get; set; }
        Guid ScheduledItemId { get; set; }
        DateTime ScheduledFor { get; set; }
    }

    public enum ScheduleTypeEnum
    {
        Post
    }
}