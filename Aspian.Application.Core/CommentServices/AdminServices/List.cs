using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.CommentServices.DTOs;
using Aspian.Application.Core.Interfaces;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.SiteModel;
using Aspian.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.CommentServices.AdminServices
{
    public class List
    {
        public class Query : IRequest<List<CommentDto>> { }

        public class Handler : IRequestHandler<Query, List<CommentDto>>
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

            public async Task<List<CommentDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var site = await _context.Sites.FirstOrDefaultAsync(x => x.SiteType == SiteTypeEnum.Blog);
                var comments = await _context.Comments.ToListAsync();

                await _logger.LogActivity(
                    site.Id,
                    ActivityCodeEnum.CommentList,
                    ActivitySeverityEnum.Information,
                    ActivityObjectEnum.Comment,
                    $"A list of all comments has requested and recieved by user");

                return _mapper.Map<List<CommentDto>>(comments);
            }
        }
    }
}