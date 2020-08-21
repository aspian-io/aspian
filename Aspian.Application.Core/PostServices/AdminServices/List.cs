using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Interfaces;
using Aspian.Application.Core.PostServices.AdminServices.DTOs;
using Aspian.Application.Core.PostServices.AdminServices.Helpers;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.PostModel;
using Aspian.Domain.SiteModel;
using Aspian.Domain.TaxonomyModel;
using Aspian.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.PostServices.AdminServices
{
    public class List
    {
        public class PostsEnvelope
        {
            public List<PostDto> Posts { get; set; }
            public int PostCount { get; set; }
        }
        public class Query : IRequest<PostsEnvelope>
        {
            // Params for pagination
            public int? Limit { get; set; }
            public int? Offset { get; set; }

            // Params for sorting
            public string Field { get; set; }
            public string Order { get; set; }

            // Params for filter
            public string FilterKey { get; set; }
            public string FilterValue { get; set; }
        }

        public class Handler : IRequestHandler<Query, PostsEnvelope>
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

            public async Task<PostsEnvelope> Handle(Query request, CancellationToken cancellationToken)
            {
                var site = await _context.Sites.FirstOrDefaultAsync(x => x.SiteType == SiteTypeEnum.Blog);
                // pagination, sorting and filtering
                var helperTDO = await PostHelpers.PaginateAndFilterAndSort(
                    _context, 
                    request.Limit, request.Offset, 
                    request.Field, request.Order, 
                    request.FilterKey, request.FilterValue
                    );

                await _logger.LogActivity(
                    site.Id,
                    ActivityCodeEnum.PostList,
                    ActivitySeverityEnum.Information,
                    ActivityObjectEnum.Post,
                    "A list of all posts has been requested and recieved by user");

                return new PostsEnvelope
                {
                    Posts = _mapper.Map<List<PostDto>>(helperTDO.PostsEnvelope),
                    PostCount = helperTDO.PostCount
                };

            }
        }
    }
}