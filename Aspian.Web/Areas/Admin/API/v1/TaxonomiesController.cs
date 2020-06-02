using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspian.Application.Core.TaxonomyService;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Aspian.Web.Areas.Admin.API.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TaxonomiesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TaxonomiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<TaxonomyDto>>> List()
        {
            return await _mediator.Send(new List.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaxonomyDto>> Details(Guid id)
        {
            return await _mediator.Send(new Details.Query{Id = id});
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create(Create.Command command)
        {
            return await _mediator.Send(command);
        }
    }
}