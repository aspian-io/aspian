using System;
using System.IO;
using System.Threading.Tasks;
using Aspian.Application.Core.AttachmentServices;
using Aspian.Application.Core.AttachmentServices.DTOs;
using MediatR;
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

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await Mediator.Send(new Delete.Command { Id = id });
        }

        [HttpPost("{id}/setmainphoto")]
        public async Task<ActionResult<Unit>> SetMainPhoto(Guid id)
        {
            return await Mediator.Send(new SetMainPhoto.Command { Id = id });
        }

        [HttpGet("images/{filename}")]
        public async Task<ActionResult> GetImage(string filename)
        {
            var getImageDto = await Mediator.Send(new GetImage.Query { FileName = filename });
            return File(getImageDto.Memory, getImageDto.MimeType, getImageDto.FileName);
        }
    }
}