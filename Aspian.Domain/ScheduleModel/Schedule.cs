using System;
using System.Collections.Generic;
using Aspian.Domain.UserModel;

namespace Aspian.Domain.ScheduleModel
{
    public class Schedule : ISchedule
    {
        public Guid Id { get; set; }
        public ScheduleTypeEnum ScheduleType { get; set; }
        public Guid ScheduledItemId { get; set; }
        public DateTime ScheduledFor { get; set; }

    }
}