using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Interfaces;
using Aspian.Application.Core.PostServices.AdminServices.DTOs;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.PostModel;
using Aspian.Domain.SiteModel;
using Aspian.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.PostServices.AdminServices
{
    public class List
    {
        public class Query : IRequest<List<PostDto>> { }

        public class Handler : IRequestHandler<Query, List<PostDto>>
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

            public async Task<List<PostDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var site = await _context.Sites.FirstOrDefaultAsync(x => x.SiteType == SiteTypeEnum.Blog);
                var posts = await _context.Posts.ToListAsync();

                await _logger.LogActivity(
                    site.Id,
                    ActivityCodeEnum.PostList,
                    ActivitySeverityEnum.Information,
                    ActivityObjectEnum.Post,
                    "A list of all posts has been requested and recieved by user");

                return _mapper.Map<List<PostDto>>(posts);
            }
        }
    }
}