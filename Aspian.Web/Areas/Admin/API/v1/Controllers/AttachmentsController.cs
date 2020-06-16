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

        [HttpGet("viewimage")]
        public FileStreamResult ViewImage(string filename)
        {
            string filepath = Path.Combine("uploads/tom/2020-06-16/photo/", filename);

            byte[] data = System.IO.File.ReadAllBytes(filepath);

            Stream stream = new MemoryStream(data);
            return new FileStreamResult(stream, "image/jpeg");
        }
    }
}