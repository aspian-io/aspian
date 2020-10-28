using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspian.Application.Core.TaxonomyServices.AdminServices;
using Aspian.Application.Core.TaxonomyServices.AdminServices.DTOs;
using Aspian.Domain.TaxonomyModel;
using Aspian.Domain.UserModel.Policy;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aspian.Web.Areas.Admin.API.v1.Controllers
{
    public class TaxonomiesController : BaseAPIController
    {
        [Authorize(Policy = AspianCorePolicy.AdminTaxonomyListPolicy)]
        [HttpGet]
        public async Task<ActionResult<List<TaxonomyDto>>> List([FromQuery] TaxonomyTypeEnum? type)
        {
            return await Mediator.Send(new List.Query{TaxonomyType = type});
        }

        [Authorize(Policy = AspianCorePolicy.AdminTaxonomyListPolicy)]
        [HttpGet("antd-category-treeselect")]
        public async Task<ActionResult<List<AntdCategoryTreeSelectDto>>> CategoryTreeSelectForAntd()
        {
            return await Mediator.Send(new CategoryTreeSelectAntd.Query());
        }

        [Authorize(Policy = AspianCorePolicy.AdminTaxonomyDetailsPolicy)]
        [HttpGet("details/{id}")]
        public async Task<ActionResult<TaxonomyDto>> Details(Guid id)
        {
            return await Mediator.Send(new Details.Query { Id = id });
        }

        [Authorize(Policy = AspianCorePolicy.AdminTaxonomyCreatePolicy)]
        [HttpPost("create")]
        public async Task<ActionResult<Unit>> Create(Create.Command command)
        {
            return await Mediator.Send(command);
        }

        [Authorize(Policy = AspianCorePolicy.AdminTaxonomyEditPolicy)]
        [HttpPut("edit/{id}")]
        public async Task<ActionResult<Unit>> Edit(Guid id, Edit.Command command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }

        [Authorize(Policy = AspianCorePolicy.AdminTaxonomyDeletePolicy)]
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await Mediator.Send(new Delete.Command { Id = id });
        }
    }
}