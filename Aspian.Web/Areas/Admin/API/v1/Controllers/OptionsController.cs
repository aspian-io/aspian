using System.Collections.Generic;
using System.Threading.Tasks;
using Aspian.Application.Core.OptionServices;
using Aspian.Application.Core.OptionServices.DTOs;
using Aspian.Domain.UserModel;
using Infrastructure.Security.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aspian.Web.Areas.Admin.API.v1.Controllers
{
    public class OptionsController : BaseAPIController
    {
        [Authorize(Policy = AspianPolicy.AdminOnly)]
        [HttpGet]
        public async Task<ActionResult<List<OptionDto>>> List()
        {
            return await Mediator.Send(new List.Query());
        }
    }
}