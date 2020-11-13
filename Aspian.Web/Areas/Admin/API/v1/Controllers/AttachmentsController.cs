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
using FluentFTP;
using Microsoft.Net.Http.Headers;

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

        [Authorize(Policy = AspianCorePolicy.AdminAttachmentListPolicy)]
        [HttpGet("filebrowser")]
        public async Task<ActionResult<List<FileBrowserDto>>> FileBrowser()
        {
            return await Mediator.Send(new FileBrowser.Query());
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

        [Authorize(Policy = AspianCorePolicy.AdminAttachmentDeletePolicy)]
        [HttpDelete("deletetusfile/{id}")]
        public async Task<ActionResult<Unit>> TusDelete(string id)
        {
            return await Mediator.Send(new TusDelete.Command { FileTusId = id });
        }

        [HttpPost("setmainphoto/{id}")]
        public async Task<ActionResult<Unit>> SetMainPhoto(Guid id)
        {
            return await Mediator.Send(new SetMainPhoto.Command { Id = id });
        }

        //[Authorize(Policy = AspianCorePolicy.AdminAttachmentGetImagePolicy)]
        [AllowAnonymous]
        [HttpGet("images/{filename}")]
        public async Task<FileStreamResult> GetImage(string filename)
        {
            var imageDto = await Mediator.Send(new GetImage.Query { FileName = filename });
            return File(imageDto.Memory, imageDto.MimeType, imageDto.FileName);
        }

        //[Authorize(Policy = AspianCorePolicy.AdminAttachmentDownloadPolicy)]
        [AllowAnonymous]
        [HttpGet("download/{filename}")]
        public async Task<FileStreamResult> Download(string filename)
        {
            var downloadFileDto = await Mediator.Send(new Download.Query { FileName = filename });
            return File(downloadFileDto.Stream, downloadFileDto.MimeType, downloadFileDto.FileName, true);
        }
    }
}