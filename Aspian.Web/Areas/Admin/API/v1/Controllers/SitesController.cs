using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspian.Application.Core.SiteServices;
using Aspian.Application.Core.SiteServices.DTOs;
using Aspian.Domain.UserModel.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aspian.Web.Areas.Admin.API.v1.Controllers
{
    [Authorize(Policy = AspianPolicy.AdminOnly)]
    public class SitesController : BaseAPIController
    {
        [HttpGet]
        public async Task<ActionResult<List<SiteDto>>> List()
        {
            return await Mediator.Send(new List.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SiteDto>> Details(Guid id)
        {
            return await Mediator.Send(new Details.Query { Id = id });
        }
    }
}