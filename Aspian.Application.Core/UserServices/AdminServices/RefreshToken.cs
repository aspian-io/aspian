using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Errors;
using Aspian.Application.Core.Interfaces;
using Aspian.Application.Core.UserServices.AdminServices.DTOs;
using Aspian.Domain.UserModel;
using Aspian.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.UserServices.AdminServices
{
    public class RefreshToken
    {
        public class Command : IRequest<UserDto>
        {
        }

        public class Handler : IRequestHandler<Command, UserDto>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IUserAccessor _userAccessor;
            private readonly UserManager<User> _userManager;
            private readonly DataContext _context;
            public Handler(DataContext context, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, IJwtGenerator jwtGenerator, IUserAccessor userAccessor)
            {
                _context = context;
                _userManager = userManager;
                _userAccessor = userAccessor;
                _jwtGenerator = jwtGenerator;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<UserDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
                if (refreshToken == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Token = "not found!" });

                var user = await _context.Users.SingleOrDefaultAsync(u => u.Tokens.Any(t => t.RefreshToken == refreshToken));
                if (user == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { User = "not found!" });

                var userClaims = await _userManager.GetClaimsAsync(user);

                var refreshTokenDto = _jwtGenerator.RefreshToken(user, refreshToken, userClaims.ToList());

                return new UserDto
                {
                    DisplayName = user.DisplayName,
                    Token = refreshTokenDto.JWT,
                    UserName = user.UserName,
                    Role = user.Role
                };
            }
        }
    }
}