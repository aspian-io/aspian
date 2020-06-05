using System.Threading.Tasks;
using Aspian.Application.Core.UserServices;
using Aspian.Application.Core.UserServices.DTOs;
using Aspian.Domain.UserModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aspian.Web.Areas.Admin.API.v1.Controllers
{
    [AllowAnonymous]
    public class UserController : BaseAPIController
    {
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(Login.Query query)
        {
            return await Mediator.Send(query);
        }
    }
}