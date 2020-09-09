using System;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Domain.ScheduleModel;

namespace Aspian.Application.Core.Interfaces
{
    public interface IScheduler
    {
        /// <summary>
        /// Sets and Queues a background task by getting <paramref name="scheduleType"/>, <paramref name="scheduleFor"/> and <paramref name="scheduledItemId"/>.
        /// </summary>
        /// <returns>
        /// A Task.
        /// </returns>
        /// <param name="scheduleType" >Type of schedule.</param>
        /// <param name="scheduleFor" >The date and time of schedule.</param>
        /// <param name="scheduledItemId" >The Guid of the item you want to schedule.</param>
        Task SetAndQueueTaskAsync(ScheduleTypeEnum scheduleType, DateTime scheduleFor, Guid scheduledItemId);

        /// <summary>
        /// Recovers all pending schedules after unexpected shutdowns.
        /// </summary>
        /// <returns>
        /// A Task.
        /// </returns>
        Task RecoverScheduledTasks();
    }
}