using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Errors;
using Aspian.Application.Core.Interfaces;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.SiteModel;
using Aspian.Domain.UserModel;
using Aspian.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.UserServices.AdminServices
{
    public class LockoutUser
    {
        public class Command : IRequest
        {
            public string UserName { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly UserManager<User> _userManager;
            private readonly IActivityLogger _logger;
            public Handler(DataContext context, UserManager<User> userManager, IActivityLogger logger)
            {
                _logger = logger;
                _userManager = userManager;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var site = await _context.Sites.SingleOrDefaultAsync(x => x.SiteType == SiteTypeEnum.Blog);
                var user = await _userManager.FindByNameAsync(request.UserName);

                if (user == null)
                    throw new RestException(HttpStatusCode.NotFound, new { User = "not found!" });

                await _userManager.SetLockoutEnabledAsync(user, true);
                var result = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));

                if (result.Succeeded) 
                {
                    await _logger.LogActivity(
                    site.Id,
                    ActivityCodeEnum.UserLockout,
                    ActivitySeverityEnum.High,
                    ActivityObjectEnum.User,
                    $"The user {user.UserName} has been locked by Admin.");

                    return Unit.Value;
                }

                throw new Exception("Problem saving changes!");
            }
        }
    }
}