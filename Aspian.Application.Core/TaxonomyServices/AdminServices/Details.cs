using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Errors;
using Aspian.Application.Core.Interfaces;
using Aspian.Application.Core.TaxonomyServices.AdminServices.DTOs;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.SiteModel;
using Aspian.Domain.TaxonomyModel;
using Aspian.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.TaxonomyServices.AdminServices
{
    public class Details
    {
        public class Query : IRequest<TaxonomyDto>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, TaxonomyDto>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IActivityLogger _logger;

            public Handler(DataContext context, IMapper mapper, IActivityLogger logger)
            {
                _logger = logger;
                _mapper = mapper;
                _context = context;
            }

            public async Task<TaxonomyDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var site = await _context.Sites.FirstOrDefaultAsync(x => x.SiteType == SiteTypeEnum.Blog);
                var taxonomy = await _context.Taxonomies.FindAsync(request.Id);

                if (taxonomy == null)
                    throw new RestException(HttpStatusCode.NotFound, new { taxonomy = "Not found!" });

                await _logger.LogActivity(
                    site.Id,
                    ActivityCodeEnum.TaxonomyDetails,
                    ActivitySeverityEnum.Information,
                    ActivityObjectEnum.Taxonomy,
                    $"The taxonomy with type of \"{taxonomy.Type.ToString()}\" and the term name \"{taxonomy.Term.Name}\" has been read.");

                return _mapper.Map<TaxonomyDto>(taxonomy);
            }
        }
    }
}