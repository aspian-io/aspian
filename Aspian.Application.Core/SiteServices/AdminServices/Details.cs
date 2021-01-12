using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Errors;
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
    public class Details
    {
        public class Query : IRequest<SiteDto>
        {
            public SiteTypeEnum SiteType { get; set; }
        }

        public class Handler : IRequestHandler<Query, SiteDto>
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

            public async Task<SiteDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var site = await _context.Sites.SingleOrDefaultAsync(x => x.SiteType == request.SiteType);

                if (site == null)
                    throw new RestException(HttpStatusCode.NotFound, new { site = "Not found!" });

                await _logger.LogActivity(
                    site.Id,
                    ActivityCodeEnum.SiteDetails,
                    ActivitySeverityEnum.Information,
                    ActivityObjectEnum.Site,
                    $"Details of the site \"{site.SiteType.ToString()}\" has been read.");

                return _mapper.Map<SiteDto>(site);
            }
        }
    }
}