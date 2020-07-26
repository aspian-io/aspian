using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
    public class List
    {
        public class Query : IRequest<List<TaxonomyDto>> { }

        public class Handler : IRequestHandler<Query, List<TaxonomyDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IActivityLogger _logger;

            public Handler(DataContext context, IMapper mapper, IActivityLogger logger)
            {
                _logger = logger;
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<TaxonomyDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var site = await _context.Sites.FirstOrDefaultAsync(x => x.SiteType == SiteTypeEnum.Blog);
                var taxonomies = await _context.Taxonomies.ToListAsync();

                await _logger.LogActivity(
                    site.Id,
                    ActivityCodeEnum.TaxonomyList,
                    ActivitySeverityEnum.Information,
                    ActivityObjectEnum.Taxonomy,
                    "A list of all taxonomies has been requested and recieved by user");

                return _mapper.Map<List<TaxonomyDto>>(taxonomies);
            }
        }
    }
}