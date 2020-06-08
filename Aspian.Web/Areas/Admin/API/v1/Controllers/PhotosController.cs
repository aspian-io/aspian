using System.Threading.Tasks;
using Aspian.Application.Core.Photos;
using Aspian.Application.Core.Photos.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Aspian.Web.Areas.Admin.API.v1.Controllers
{
    public class PhotosController : BaseAPIController
    {
        [HttpPost]
        public async Task<ActionResult<PhotoDto>> Add([FromForm] Add.Command command)
        {
            return await Mediator.Send(command);
        }
    }
}