using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Interfaces;
using Aspian.Application.Core.UserServices.DTOs;
using Aspian.Domain.UserModel;
using Aspian.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aspian.Application.Core.UserServices
{
    public class CurrentUser
    {
        public class Query : IRequest<UserDto> { }

        public class Handler : IRequestHandler<Query, UserDto>
        {
            private readonly UserManager<User> _userManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IUserAccessor _userAccessor;
            public Handler(UserManager<User> userManager, IJwtGenerator jwtGenerator, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _jwtGenerator = jwtGenerator;
                _userManager = userManager;

            }

            public async Task<UserDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());

                return new UserDto
                {
                    DisplayName = user.DisplayName,
                    UserName = user.UserName,
                    //Token = _jwtGenerator.CreateToken(user),
                    Image = null
                };
            }
        }
    }
}