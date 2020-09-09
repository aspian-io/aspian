using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Interfaces;
using Aspian.Domain.PostModel;
using Aspian.Domain.ScheduleModel;
using Aspian.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Schedule
{
    public class ScheduleTask : IScheduler, IDisposable
    {
        private readonly ILogger<ScheduleTask> _logger;
        private Timer _timer;
        public ScheduleTask(IBackgroundTaskQueue taskQueue, IServiceProvider services, ILogger<ScheduleTask> logger)
        {
            TaskQueue = taskQueue;
            Services = services;
            _logger = logger;
        }

        private IBackgroundTaskQueue TaskQueue { get; }
        private IServiceProvider Services { get; }
        //private DateTime ScheduleFor { get; set; }
        private List<Aspian.Domain.ScheduleModel.Schedule> Schedules = new List<Aspian.Domain.ScheduleModel.Schedule>();

        // Set task to schedule for a specific date and time
        public async Task SetAndQueueTaskAsync(ScheduleTypeEnum scheduleType, DateTime scheduleFor, Guid scheduledItemId)
        {
            await StoreScheduledItemInfo(scheduleType, scheduleFor, scheduledItemId);
            TaskQueue.QueueBackgroundWorkItem(SetTimer);
        }

        // Rocovers scheduled tasks after unexpected shutdown
        public async Task RecoverScheduledTasks()
        {
            using (var scope = Services.CreateScope())
            {
                var context =
                scope.ServiceProvider
                    .GetRequiredService<DataContext>();

                var scheduledTasks = await context.Schedules.OrderBy(x => x.ScheduledFor).ToListAsync();
                if (scheduledTasks.Count > 0)
                {
                    foreach (var item in scheduledTasks)
                    {
                        if ((item.ScheduledFor - DateTime.UtcNow) <= TimeSpan.Zero)
                        {
                            await ScheduledItemChangeState(item.ScheduledFor);
                        }
                        else
                        {
                            Schedules.Add(item);

                        }
                    }
                }

            }
            if (Schedules.Count > 0) TaskQueue.QueueBackgroundWorkItem(SetTimer);
        }

        // Stores scheduling information to database
        private async Task StoreScheduledItemInfo(ScheduleTypeEnum scheduleType, DateTime scheduleFor, Guid scheduledItemId)
        {
            using (var scope = Services.CreateScope())
            {
                var context =
                scope.ServiceProvider
                    .GetRequiredService<DataContext>();

                var schedule = new Aspian.Domain.ScheduleModel.Schedule
                {
                    ScheduleType = scheduleType,
                    ScheduledFor = scheduleFor,
                    ScheduledItemId = scheduledItemId
                };

                context.Schedules.Add(schedule);

                await context.SaveChangesAsync();
                _logger.LogInformation("Scheduled item information saved successfully.");
                Schedules.Add(schedule);
            }
        }

        // Set timer for schedule item
        private Task SetTimer(CancellationToken stoppingToken)
        {
            if (Schedules.Count > 0)
            {
                foreach (var item in Schedules)
                {
                    _logger.LogInformation($"Post scheduled for: {item.ScheduledFor}.");

                    if ((item.ScheduledFor - DateTime.UtcNow) <= TimeSpan.Zero)
                    {
                        ScheduledItemChangeState(item.ScheduledFor).Wait();
                    }
                    else
                    {
                        _timer = new Timer(DoWork, null, (item.ScheduledFor - DateTime.UtcNow).Duration(), TimeSpan.Zero);
                    }

                }
            }
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var utcDateTimeNow = DateTime.UtcNow;
            ScheduledItemChangeState(utcDateTimeNow).Wait();
        }

        // Changes after the scheduled time comes
        private async Task ScheduledItemChangeState(DateTime scheduleFor)
        {
            using (var scope = Services.CreateScope())
            {
                var context =
                scope.ServiceProvider
                    .GetRequiredService<DataContext>();

                var scheduledItem = await context.Schedules.FirstOrDefaultAsync(x => x.ScheduledFor <= scheduleFor);
                if (scheduledItem != null)
                {
                    switch (scheduledItem.ScheduleType)
                    {
                        case ScheduleTypeEnum.ScheduledPost:
                            var post = await context.Posts.FindAsync(scheduledItem.ScheduledItemId);
                            if (post != null)
                            {
                                post.PostStatus = PostStatusEnum.Publish;
                                post.ScheduledFor = null;
                                context.Schedules.Remove(scheduledItem);
                                await context.SaveChangesAsync();
                            }
                            break;

                        default:
                            break;
                    }
                    _logger.LogInformation("Scheduled item state has been changed successfully.");
                }
                else
                {
                    _logger.LogInformation("Scheduled item not found!");
                }
            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}