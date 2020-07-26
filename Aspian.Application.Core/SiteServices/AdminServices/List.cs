using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Interfaces;
using Aspian.Application.Core.SiteServices.AdminServices.DTOs;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.SiteModel;
using Aspian.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.SiteServices.AdminServices
{
    public class List
    {
        public class Query : IRequest<List<SiteDto>> { }

        public class Handler : IRequestHandler<Query, List<SiteDto>>
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

            public async Task<List<SiteDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var site = await _context.Sites.SingleOrDefaultAsync(x => x.SiteType == SiteTypeEnum.Blog);
                var Sites = await _context.Sites.ToListAsync();

                await _logger.LogActivity(
                    site.Id,
                    ActivityCodeEnum.SiteList,
                    ActivitySeverityEnum.Information,
                    ActivityObjectEnum.Site,
                    "Get a List of all sites infromation.");

                return _mapper.Map<List<SiteDto>>(Sites);
            }
        }
    }
}