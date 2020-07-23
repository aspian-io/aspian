using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspian.Application.Core.CommentServices.AdminServices;
using Aspian.Application.Core.CommentServices.DTOs;
using Aspian.Domain.UserModel.Policy;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aspian.Web.Areas.Admin.API.v1.Controllers
{
    public class CommentsController : BaseAPIController
    {
        [Authorize(Policy = AspianCorePolicy.AdminCommentListPolicy)]
        [HttpGet]
        public async Task<ActionResult<List<CommentDto>>> List()
        {
            return await Mediator.Send(new List.Query());
        }
        
        [Authorize(Policy = AspianCorePolicy.AdminCommentCreatePolicy)]
        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Create.Command command)
        {
            return await Mediator.Send(command);
        }
        
        [Authorize(Policy = AspianCorePolicy.AdminCommentEditPolicy)]
        [HttpPut("edit/{id}")]
        public async Task<ActionResult<Unit>> Edit(Guid id, Edit.Command command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }

        [Authorize(Policy = AspianCorePolicy.AdminCommentApprovePolicy)]
        [HttpPut("approve/{id}")]
        public async Task<ActionResult<Unit>> Approve(Guid id)
        {
            return await Mediator.Send(new Approve.Command { Id = id });
        }

        [Authorize(Policy = AspianCorePolicy.AdminCommentUnapprovePolicy)]
        [HttpPut("unapprove/{id}")]
        public async Task<ActionResult<Unit>> Unapprove(Guid id)
        {
            return await Mediator.Send(new Unapprove.Command { Id = id });
        }
        
        [Authorize(Policy = AspianCorePolicy.AdminCommentDetailsPolicy)]
        [HttpGet("details/{id}")]
        public async Task<ActionResult<CommentDto>> Details(Guid id)
        {
            return await Mediator.Send(new Details.Query { Id = id });
        }

        [Authorize(Policy = AspianCorePolicy.AdminCommentDeletePolicy)]
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await Mediator.Send(new Delete.Command { Id = id });
        }
    }
}