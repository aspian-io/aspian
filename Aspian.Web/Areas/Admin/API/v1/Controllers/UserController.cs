using System.Threading.Tasks;
using Aspian.Application.Core.UserServices.AdminServices;
using Aspian.Application.Core.UserServices.DTOs;
using Aspian.Domain.UserModel;
using Aspian.Domain.UserModel.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(Register.Command command)
        {
            return await Mediator.Send(command);
        }

        [Authorize(Policy = AspianCorePolicy.AdminUserCurrentPolicy)]
        [HttpGet]
        public async Task<ActionResult<UserDto>> CurrentUser()
        {
            return await Mediator.Send(new CurrentUser.Query());
        }

        [Authorize(Policy = AspianCorePolicy.AdminUserProfilePolicy)]
        [HttpGet("{username}")]
        public async Task<ActionResult<UserProfileDto>> Profile(string username)
        {
            return await Mediator.Send(new UserProfile.Query { UserName = username });
        }
    }
}