using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Interfaces;
using Aspian.Application.Core.OptionServices.DTOs;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.OptionModel;
using Aspian.Domain.SiteModel;
using Aspian.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.OptionServices.AdminServices
{
    public class List
    {
        public class Query : IRequest<List<OptionDto>> { }

        public class Handler : IRequestHandler<Query, List<OptionDto>>
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

            public async Task<List<OptionDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var site = await _context.Sites.FirstOrDefaultAsync(x => x.SiteType == SiteTypeEnum.Blog);
                var options = await _context.Options.ToListAsync();

                await _logger.LogActivity(
                    site.Id,
                    ActivityCodeEnum.OptionList,
                    ActivitySeverityEnum.Medium,
                    ActivityObjectEnum.Option,
                    $"A list of all options has requested and recieved by user");

                return _mapper.Map<List<OptionDto>>(options);
            }
        }
    }
}