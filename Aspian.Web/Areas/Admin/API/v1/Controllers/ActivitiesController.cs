using System.Collections.Generic;
using System.Threading.Tasks;
using Aspian.Application.Core.ActivityServices.AdminServices;
using Aspian.Application.Core.ActivityServices.AdminServices.DTOs;
using Aspian.Domain.UserModel.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aspian.Web.Areas.Admin.API.v1.Controllers
{
    public class ActivitiesController : BaseAPIController
    {
        [Authorize(Policy = AspianCorePolicy.AdminActivityListPolicy)]
        [HttpGet]
        public async Task<ActionResult<List<ActivityDto>>> List()
        {
            return await Mediator.Send(new List.Query());
        }
    }
}