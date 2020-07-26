using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspian.Application.Core.AttachmentServices.AdminServices;
using Aspian.Application.Core.AttachmentServices.UserServices;
using Aspian.Application.Core.AttachmentServices.AdminServices.DTOs;
using Aspian.Domain.UserModel.Policy;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aspian.Web.Areas.Admin.API.v1.Controllers
{
    public class AttachmentsController : BaseAPIController
    {
        [Authorize(Policy = AspianCorePolicy.AdminAttachmentListPolicy)]
        [HttpGet]
        public async Task<ActionResult<List<AttachmentDto>>> List()
        {
            return await Mediator.Send(new List.Query());
        }

        [Authorize(Policy = AspianCorePolicy.AdminAttachmentAddPolicy)]
        [HttpPost]
        public async Task<ActionResult<AttachmentDto>> Add([FromForm] Add.Command command)
        {
            return await Mediator.Send(command);
        }

        [Authorize(Policy = AspianCorePolicy.AdminAttachmentDeletePolicy)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await Mediator.Send(new Delete.Command { Id = id });
        }

        [HttpPost("setmainphoto/{id}")]
        public async Task<ActionResult<Unit>> SetMainPhoto(Guid id)
        {
            return await Mediator.Send(new SetMainPhoto.Command { Id = id });
        }

        [Authorize(Policy = AspianCorePolicy.AdminAttachmentGetImagePolicy)]
        [HttpGet("images/{filename}")]
        public async Task<ActionResult> GetImage(string filename)
        {
            var imageDto = await Mediator.Send(new GetImage.Query { FileName = filename });
            return File(imageDto.Memory, imageDto.MimeType, imageDto.FileName);
        }

        [Authorize(Policy = AspianCorePolicy.AdminAttachmentDownloadPolicy)]
        [HttpGet("download/{filename}")]
        public async Task<ActionResult> Download(string filename)
        {
            var downloadFileDto = await Mediator.Send(new Download.Query { FileName = filename });
            return File(downloadFileDto.Memory, downloadFileDto.MimeType, downloadFileDto.FileName);
        }
    }
}