using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspian.Application.Core.SiteServices.AdminServices;
using Aspian.Application.Core.SiteServices.AdminServices.DTOs;
using Aspian.Domain.UserModel.Policy;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aspian.Web.Areas.Admin.API.v1.Controllers
{
    public class SitesController : BaseAPIController
    {
        [Authorize(Policy = AspianCorePolicy.AdminSiteListPolicy)]
        [HttpGet]
        public async Task<ActionResult<List<SiteDto>>> List()
        {
            return await Mediator.Send(new List.Query());
        }

        [Authorize(Policy = AspianCorePolicy.AdminSiteDetailsPolicy)]
        [HttpGet("{id}")]
        public async Task<ActionResult<SiteDto>> Details(Guid id)
        {
            return await Mediator.Send(new Details.Query { Id = id });
        }

        [Authorize(Policy = AspianCorePolicy.AdminSiteEditPolicy)]
        [HttpPost("edit/{id}")]
        public async Task<ActionResult<Unit>> Edit(Guid id, Edit.Command command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }

        [Authorize(Policy = AspianCorePolicy.DeveloperOnlyPolicy)]
        [HttpPost("developer-edit/{id}")]
        public async Task<ActionResult<Unit>> DeveloperEdit(Guid id, DeveloperEdit.Command command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }
    }
}