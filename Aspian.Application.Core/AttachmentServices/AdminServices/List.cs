using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.AttachmentServices.AdminServices.DTOs;
using Aspian.Application.Core.Interfaces;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.SiteModel;
using Aspian.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.AttachmentServices.AdminServices
{
    public class List
    {
        public class Query : IRequest<List<AttachmentDto>> { }

        public class Handler : IRequestHandler<Query, List<AttachmentDto>>
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

            public async Task<List<AttachmentDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var site = await _context.Sites.FirstOrDefaultAsync(x => x.SiteType == SiteTypeEnum.Blog);
                var attachments = await _context.Attachments.ToListAsync();

                await _logger.LogActivity(
                        site.Id,
                        ActivityCodeEnum.AttachmentList,
                        ActivitySeverityEnum.Information,
                        ActivityObjectEnum.Attachemnt,
                        "Get list of all attachments");

                return _mapper.Map<List<AttachmentDto>>(attachments);
            }
        }
    }
}