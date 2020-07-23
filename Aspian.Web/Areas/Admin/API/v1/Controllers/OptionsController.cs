using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspian.Application.Core.OptionServices.AdminServices;
using Aspian.Application.Core.OptionServices.DTOs;
using Aspian.Domain.UserModel.Policy;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aspian.Web.Areas.Admin.API.v1.Controllers
{
    public class OptionsController : BaseAPIController
    {
        [Authorize(Policy = AspianCorePolicy.AdminOptionListPolicy)]
        [HttpGet]
        public async Task<ActionResult<List<OptionDto>>> List()
        {
            return await Mediator.Send(new List.Query());
        }

        [Authorize(Policy = AspianCorePolicy.AdminOptionEditPolicy)]
        [HttpPut("edit/{id}")]
        public async Task<ActionResult<Unit>> Edit(Guid id, Edit.Command command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }

        [Authorize(Policy = AspianCorePolicy.AdminOptionRestoreDefaultPolicy)]
        [HttpPut("restoredefaultoption/{id}")]
        public async Task<ActionResult<Unit>> RestoreDefaultOption(Guid id)
        {
            return await Mediator.Send(new RestoreDefaultOption.Command{Id = id});
        }

        [Authorize(Policy = AspianCorePolicy.AdminOptionRestoreDefaultPolicy)]
        [HttpPut("restoredefaultoptions")]
        public async Task<ActionResult<Unit>> RestoreDefaultOptions(RestoreDefaultOptions.Command ids)
        {
            return await Mediator.Send(ids);
        }
    }
}