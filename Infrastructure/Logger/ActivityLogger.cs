using System;
using System.Linq;
using System.Threading.Tasks;
using Aspian.Application.Core.Interfaces;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.OptionModel;
using Aspian.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Logger
{
    public class ActivityLogger : IActivityLogger
    {
        private readonly DataContext _context;
        public ActivityLogger(DataContext context)
        {
            _context = context;
        }

        public async Task LogActivity(Guid siteId, ActivityCodeEnum code, ActivitySeverityEnum severity, ActivityObjectEnum objectName, string message)
        {
            var activity = new Activity
            {
                Code = code,
                Severity = severity,
                ObjectName = objectName,
                Message = message,
                SiteId = siteId
            };

            var loggingActivityOption = await _context.Optionmetas.FirstOrDefaultAsync(x => x.Key == KeyEnum.Activity__LoggingActivities);
            if (loggingActivityOption.Value == ValueEnum.Activity__LoggingActivities_Enable)
            {
                // Add new activity log
                _context.Activities.Add(activity);

                var pruningActivityOption = await _context.Optionmetas.FirstOrDefaultAsync(x => x.Key == KeyEnum.Activity__Pruning);
                if (pruningActivityOption.Value == ValueEnum.Activity__PruningActivities_Enable)
                {
                    var pruningDateOption = await _context.Optionmetas.FirstOrDefaultAsync(x => x.Key == KeyEnum.Activity__PruningDate);
                    switch (pruningDateOption.Value)
                    {
                        case ValueEnum.Activity__PruningDate_EveryWeek:
                            var activitiesToRemoveWeekly = _context.Activities.Where(x => x.CreatedAt < DateTime.Now.AddDays(-7));
                            
                            if (activitiesToRemoveWeekly != null)
                                _context.Activities.RemoveRange(activitiesToRemoveWeekly);
                            break;

                        case ValueEnum.Activity__PruningDate_EveryMonth:
                            var activitiesToRemoveMonthly = _context.Activities.Where(x => x.CreatedAt < DateTime.Now.AddMonths(-1));
                            
                            if (activitiesToRemoveMonthly != null)
                                _context.Activities.RemoveRange(activitiesToRemoveMonthly);
                            break;

                        case ValueEnum.Activity__PruningDate_EverySixMonths:
                            var activitiesToRemoveEverySixMonths = _context.Activities.Where(x => x.CreatedAt < DateTime.Now.AddMonths(-6));
                            
                            if (activitiesToRemoveEverySixMonths != null)
                                _context.Activities.RemoveRange(activitiesToRemoveEverySixMonths);
                            break;

                        case ValueEnum.Activity__PruningDate_EveryYear:
                            var activitiesToRemoveYearly = _context.Activities.Where(x => x.CreatedAt < DateTime.Now.AddYears(-1));
                            
                            if (activitiesToRemoveYearly != null)
                                _context.Activities.RemoveRange(activitiesToRemoveYearly);
                            break;
                    }

                }

                var suceeded = await _context.SaveChangesAsync() > 0;

                if (!suceeded)
                    throw new Exception("Problem logging activity!");

            }
        }
    }
}