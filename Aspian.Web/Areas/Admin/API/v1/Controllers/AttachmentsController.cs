using System.Threading.Tasks;
using Aspian.Application.Core.AttachmentServices;
using Aspian.Application.Core.AttachmentServices.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Aspian.Web.Areas.Admin.API.v1.Controllers
{
    public class AttachmentsController : BaseAPIController
    {
        [HttpPost]
        public async Task<ActionResult<AttachmentDto>> Add([FromForm] Add.Command command)
        {
            return await Mediator.Send(command);
        }
    }
}