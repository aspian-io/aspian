using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspian.Application.Core.PostServices.AdminServices;
using Aspian.Application.Core.PostServices.AdminServices.DTOs;
using Aspian.Domain.PostModel;
using Aspian.Domain.UserModel.Policy;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aspian.Web.Areas.Admin.API.v1.Controllers
{
    public class PostsController : BaseAPIController
    {
        [Authorize(Policy = AspianCorePolicy.AdminPostListPolicy)]
        [HttpGet]
        public async Task<ActionResult<List<PostDto>>> List()
        {
            return await Mediator.Send(new List.Query());
        }

        [Authorize(Policy = AspianCorePolicy.AdminPostCreatePolicy)]
        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Create.Command command)
        {
            return await Mediator.Send(command);
        }

        [Authorize(Policy = AspianCorePolicy.AdminPostDetailsPolicy)]
        [HttpGet("{id}")]
        public async Task<ActionResult<PostDto>> Details(Guid id)
        {
            return await Mediator.Send(new Details.Query { Id = id });
        }

        [Authorize(Policy = AspianCorePolicy.AdminPostEditPolicy)]
        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Edit(Guid id, Edit.Command command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }

        [Authorize(Policy = AspianCorePolicy.AdminPostDeletePolicy)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await Mediator.Send(new Delete.Command { Id = id });
        }
    }
}