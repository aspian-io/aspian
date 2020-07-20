using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspian.Application.Core.OptionServices;
using Aspian.Application.Core.OptionServices.DTOs;
using Aspian.Domain.UserModel.Policy;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aspian.Web.Areas.Admin.API.v1.Controllers
{
    [Authorize(Policy = AspianPolicy.AdminOnly)]
    public class OptionsController : BaseAPIController
    {
        [HttpGet]
        public async Task<ActionResult<List<OptionDto>>> List()
        {
            return await Mediator.Send(new List.Query());
        }

        [HttpPut("edit/{id}")]
        public async Task<ActionResult<Unit>> Edit(Guid id, Edit.Command command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }

        [HttpPut("restoredefaultoption/{id}")]
        public async Task<ActionResult<Unit>> RestoreDefaultOption(Guid id)
        {
            return await Mediator.Send(new RestoreDefaultOption.Command{Id = id});
        }

        [HttpPut("restoredefaultoptions")]
        public async Task<ActionResult<Unit>> RestoreDefaultOptions(RestoreDefaultOptions.Command ids)
        {
            return await Mediator.Send(ids);
        }
    }
}