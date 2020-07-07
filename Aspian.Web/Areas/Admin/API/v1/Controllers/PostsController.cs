using System.Threading.Tasks;
using Aspian.Application.Core.PostServices;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Aspian.Web.Areas.Admin.API.v1.Controllers
{
    public class PostsController : BaseAPIController
    {
        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Create.Command command)
        {
            return await Mediator.Send(command);
        }
    }
}