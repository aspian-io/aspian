using System.Threading.Tasks;
using Aspian.Application.Core.UserServices.AdminServices;
using Aspian.Application.Core.UserServices.AdminServices.DTOs;
using Aspian.Domain.UserModel.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MediatR;

namespace Aspian.Web.Areas.Admin.API.v1.Controllers
{
    public class UserController : BaseAPIController
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(Login.Query query)
        {
            return await Mediator.Send(query);
        }

        [AllowAnonymous]
        [HttpGet("refresh-token")]
        public async Task<ActionResult<UserDto>> RefreshToken()
        {
            return await Mediator.Send(new RefreshToken.Command());
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(Register.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<UserProfileDto>> Profile(string username)
        {
            return await Mediator.Send(new UserProfile.Query { UserName = username });
        }

        [Authorize(Policy = AspianCorePolicy.AdminUserCurrentPolicy)]
        [HttpGet]
        public async Task<ActionResult<CurrentUserDto>> CurrentUser()
        {
            return await Mediator.Send(new CurrentUser.Query());
        }

        [HttpGet("list")]
        public async Task<ActionResult<List<UserListDto>>> List()
        {
            return await Mediator.Send(new List.Query());
        }

        [Authorize(Policy = AspianCorePolicy.AdminUserLockoutPolicy)]
        [HttpPost("lockout/{username}")]
        public async Task<ActionResult<Unit>> Lockout(string username)
        {
            return await Mediator.Send(new LockoutUser.Command{UserName = username});
        }

        [Authorize(Policy = AspianCorePolicy.AdminUserUnlockPolicy)]
        [HttpPost("unlock/{username}")]
        public async Task<ActionResult<Unit>> Unlock(string username)
        {
            return await Mediator.Send(new UnlockUser.Command{UserName = username});
        }

        [HttpPost("edit-profile")]
        public async Task<ActionResult<Unit>> EditProfile(EditProfile.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost("change-password")]
        public async Task<ActionResult<Unit>> ChangePassword(ChangePassword.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost("logout")]
        public void Logout()
        {
            Response.Cookies.Delete("refreshToken");
        }
    }
}