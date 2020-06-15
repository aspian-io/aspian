using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Errors;
using Aspian.Application.Core.Interfaces;
using Aspian.Application.Core.UserServices.DTOs;
using Aspian.Application.Core.Validators;
using Aspian.Domain.UserModel;
using Aspian.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.UserServices
{
    public class Register
    {
        public class Command : IRequest<UserDto>
        {
            public string DisplayName { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.DisplayName).NotEmpty();
                RuleFor(x => x.UserName).NotEmpty();
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).Password();
            }
        }

        public class Handler : IRequestHandler<Command, UserDto>
        {
            private readonly DataContext _context;
            private readonly UserManager<User> _userManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly RoleManager<IdentityRole> _roleManager;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, UserManager<User> userManager, IJwtGenerator jwtGenerator, RoleManager<IdentityRole> roleManager, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _roleManager = roleManager;
                _jwtGenerator = jwtGenerator;
                _userManager = userManager;
                _context = context;
            }

            public async Task<UserDto> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await _context.Users.Where(x => x.Email == request.Email).AnyAsync())
                    throw new RestException(HttpStatusCode.BadRequest, new { Email = "Email already exists!" });

                if (await _context.Users.Where(x => x.UserName == request.UserName).AnyAsync())
                    throw new RestException(HttpStatusCode.BadRequest, new { Username = "Username already exists!" });

                var user = new User
                {
                    DisplayName = request.UserName,
                    Email = request.Email,
                    UserName = request.UserName
                };

                var createUserResult = await _userManager.CreateAsync(user, request.Password);

                if (createUserResult.Succeeded)
                {
                    return new UserDto
                    {
                        DisplayName = user.DisplayName,
                        Token = _jwtGenerator.CreateToken(user, claim: null),
                        UserName = user.UserName,
                        Image = null,
                        IP = _userAccessor.GetCurrentUserIpAddress(),
                        Agent = _userAccessor.GetCurrentUserAgent()
                    };
                }

                throw new Exception("Problem creating user!");
            }
        }
    }
}