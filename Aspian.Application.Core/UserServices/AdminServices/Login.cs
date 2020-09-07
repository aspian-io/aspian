using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Errors;
using Aspian.Application.Core.Interfaces;
using Aspian.Application.Core.UserServices.AdminServices.DTOs;
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
    public class Login
    {
        public class Query : IRequest<UserDto>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, UserDto>
        {
            private readonly UserManager<User> _userManager;
            private readonly SignInManager<User> _signInManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IActivityLogger _logger;
            private readonly DataContext _context;
            public Handler(DataContext context, UserManager<User> userManager, SignInManager<User> signInManager, IJwtGenerator jwtGenerator, IActivityLogger logger)
            {
                _context = context;
                _logger = logger;
                _jwtGenerator = jwtGenerator;
                _signInManager = signInManager;
                _userManager = userManager;
            }

            public async Task<UserDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var site = await _context.Sites.FirstOrDefaultAsync(x => x.SiteType == SiteTypeEnum.Blog);
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null)
                {
                    await _logger.LogActivity(
                    site.Id,
                    ActivityCodeEnum.UserLogin,
                    ActivitySeverityEnum.Medium,
                    ActivityObjectEnum.User,
                    "Login failed. The requested email address or username does not exist.");

                    throw new RestException(HttpStatusCode.Unauthorized);
                }
                user.LastLoginDate = DateTime.UtcNow;

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

                if (result.Succeeded)
                {
                    user.LastLoginDate = DateTime.UtcNow;
                    await _context.SaveChangesAsync();

                    await _logger.LogActivity(
                    site.Id,
                    ActivityCodeEnum.UserLogin,
                    ActivitySeverityEnum.Low,
                    ActivityObjectEnum.User,
                    $"A user with the username \"{user.UserName}\" has been logged in successfully.");

                    var claims = await _userManager.GetClaimsAsync(user);
                    return new UserDto
                    {
                        DisplayName = user.DisplayName,
                        Token = _jwtGenerator.CreateToken(user, claims.ToList()),
                        UserName = user.UserName,
                        Role = user.Role,
                        ProfilePhotoName = user.CreatedAttachments?.FirstOrDefault(x => x.Type == AttachmentTypeEnum.Photo && x.IsMain).FileName
                    };
                }

                await _logger.LogActivity(
                    site.Id,
                    ActivityCodeEnum.UserLogin,
                    ActivitySeverityEnum.Medium,
                    ActivityObjectEnum.User,
                    "Login faild. Password is incorrect!");

                throw new RestException(HttpStatusCode.Unauthorized);
            }
        }
    }
}