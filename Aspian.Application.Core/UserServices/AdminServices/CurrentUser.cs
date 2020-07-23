using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Interfaces;
using Aspian.Application.Core.UserServices.DTOs;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.AttachmentModel;
using Aspian.Domain.SiteModel;
using Aspian.Domain.UserModel;
using Aspian.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.UserServices.AdminServices
{
    public class CurrentUser
    {
        public class Query : IRequest<UserDto> { }

        public class Handler : IRequestHandler<Query, UserDto>
        {
            private readonly UserManager<User> _userManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IUserAccessor _userAccessor;
            private readonly IActivityLogger _logger;
            private readonly DataContext _context;
            public Handler(DataContext context, UserManager<User> userManager, IJwtGenerator jwtGenerator, IUserAccessor userAccessor, IActivityLogger logger)
            {
                _context = context;
                _logger = logger;
                _userAccessor = userAccessor;
                _jwtGenerator = jwtGenerator;
                _userManager = userManager;

            }

            public async Task<UserDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var site = await _context.Sites.FirstOrDefaultAsync(x => x.SiteType == SiteTypeEnum.Blog);
                var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());
                var claims = await _userManager.GetClaimsAsync(user);

                await _logger.LogActivity(
                    site.Id,
                    ActivityCodeEnum.UserCurrent,
                    ActivitySeverityEnum.Information,
                    ActivityObjectEnum.User,
                    $"Current user info of a user with the username \"{user.UserName}\" has been requested and received.");

                return new UserDto
                {
                    DisplayName = user.DisplayName,
                    UserName = user.UserName,
                    Token = _jwtGenerator.CreateToken(user, claims.ToList()),
                    //Image = user.CreatedAttachments.FirstOrDefault(x => x.Type == AttachmentTypeEnum.Photo && x.IsMain)?.Url
                };
            }
        }
    }
}