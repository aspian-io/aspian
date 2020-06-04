using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Errors;
using Aspian.Application.Core.UserServices.DTOs;
using Aspian.Domain.UserModel;
using Aspian.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aspian.Application.Core.UserServices
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
            public Handler(UserManager<User> userManager, SignInManager<User> signInManager)
            {
                _signInManager = signInManager;
                _userManager = userManager;
            }

            public async Task<UserDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null)
                    throw new RestException(HttpStatusCode.Unauthorized);

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

                if (result.Succeeded)
                {
                    // TODO: generate token
                    return new UserDto
                    {
                        DisplayName = user.DisplayName,
                        Token = "This will be a token",
                        UserName = user.UserName,
                        Image = null
                    };
                }

                throw new RestException(HttpStatusCode.Unauthorized);
            }
        }
    }
}