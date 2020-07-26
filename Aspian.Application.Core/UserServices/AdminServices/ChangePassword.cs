using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Errors;
using Aspian.Application.Core.Interfaces;
using Aspian.Application.Core.Validators;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.SiteModel;
using Aspian.Domain.UserModel;
using Aspian.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.UserServices.AdminServices
{
    public class ChangePassword
    {
        public class Command : IRequest
        {
            public string CurrentPassword { get; set; }
            public string NewPassword { get; set; }
            public string ConfirmNewPassword { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.CurrentPassword).Password();
                RuleFor(x => x.NewPassword).Password();
                RuleFor(x => x.ConfirmNewPassword).Password();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly IActivityLogger _logger;
            private readonly UserManager<User> _userManager;
            public Handler(DataContext context, IUserAccessor userAccessor, UserManager<User> userManager, IActivityLogger logger)
            {
                _userManager = userManager;
                _logger = logger;
                _userAccessor = userAccessor;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var site = await _context.Sites.SingleOrDefaultAsync(x => x.SiteType == SiteTypeEnum.Blog);
                var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());

                if (request.NewPassword != request.ConfirmNewPassword)
                    throw new RestException(HttpStatusCode.BadRequest, new { ConfirmPassword = "is not matched!" });

                var currentPasswordVerfified = await _userManager.CheckPasswordAsync(user, request.CurrentPassword);

                if (!currentPasswordVerfified)
                    throw new RestException(HttpStatusCode.BadRequest, new { CurrentPassword = "is incorrect!" });
                
                var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

                if (result.Succeeded) 
                {
                    await _logger.LogActivity(
                    site.Id,
                    ActivityCodeEnum.UserChangePassword,
                    ActivitySeverityEnum.High,
                    ActivityObjectEnum.User,
                    "User account's password has been changed.");

                    return Unit.Value;
                }

                throw new Exception("Problem saving changes!");
            }
        }
    }
}