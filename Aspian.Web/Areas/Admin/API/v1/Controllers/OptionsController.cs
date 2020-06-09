using System.Collections.Generic;
using System.Threading.Tasks;
using Aspian.Application.Core.OptionServices;
using Aspian.Application.Core.OptionServices.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Aspian.Web.Areas.Admin.API.v1.Controllers
{
    public class OptionsController : BaseAPIController
    {
        [HttpGet]
        public async Task<ActionResult<List<OptionDto>>> List()
        {
            return await Mediator.Send(new List.Query());
        }
    }
}