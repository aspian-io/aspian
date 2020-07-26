using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Errors;
using Aspian.Application.Core.Interfaces;
using Aspian.Application.Core.Validators;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.AttachmentModel;
using Aspian.Domain.SiteModel;
using Aspian.Domain.UserModel;
using Aspian.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.UserServices.AdminServices
{
    public class EditProfile
    {
        public class Command : IRequest
        {
            public string DisplayName { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.DisplayName).NotEmpty();
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Role).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly UserManager<User> _userManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IActivityLogger _logger;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor, UserManager<User> userManager, IJwtGenerator jwtGenerator, IActivityLogger logger)
            {
                _userAccessor = userAccessor;
                _logger = logger;
                _jwtGenerator = jwtGenerator;
                _userManager = userManager;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var site = await _context.Sites.SingleOrDefaultAsync(x => x.SiteType == SiteTypeEnum.Blog);
                var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());

                user.DisplayName = request.DisplayName;
                user.Role = request.Role;

                var success = await _context.SaveChangesAsync() > 0;

                if (await _context.Users.Where(x => x.Id != user.Id && x.Email == request.Email).AnyAsync())
                    throw new RestException(HttpStatusCode.BadRequest, new { Email = "already exists!" });

                var changeEmailToken = await _userManager.GenerateChangeEmailTokenAsync(user, request.Email);
                var changeEmailResult = await _userManager.ChangeEmailAsync(user, request.Email, changeEmailToken);

                if (!changeEmailResult.Succeeded)
                    throw new Exception("Problem changing email address!");

                

                if (success && changeEmailResult.Succeeded) 
                {
                    await _logger.LogActivity(
                    site.Id,
                    ActivityCodeEnum.UserEditProfile,
                    ActivitySeverityEnum.Low,
                    ActivityObjectEnum.User,
                    "User's profile info has been edited.");
                    
                    return Unit.Value;
                }

                throw new Exception("Problem saving changes!");
            }
        }
    }
}