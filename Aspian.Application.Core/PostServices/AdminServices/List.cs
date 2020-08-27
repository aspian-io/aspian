using System;
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
            public int MaxAttachmentsNumber { get; set; }
            public int MaxViewCount { get; set; }
            public int MaxPostHistories { get; set; }
            public int MaxComments { get; set; }
            public int MaxChildPosts { get; set; }
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

            // Params for DateRange filter
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }

            // Params for slider filter
            public int? StartNumber { get; set; }
            public int? EndNumber { get; set; }
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
                    request.FilterKey, request.FilterValue,
                    request.StartDate, request.EndDate,
                    request.StartNumber, request.EndNumber
                    );
                var maxAttachmentsNumber = helperTDO.PostCount > 0 ? _context.Posts.OrderByDescending(x => x.PostAttachments.Count).First().PostAttachments.Count : 0;
                var maxViewCount = helperTDO.PostCount > 0 ? await _context.Posts.MaxAsync(x => x.ViewCount) : 0;
                var maxPostHistories = helperTDO.PostCount > 0 ? _context.Posts.OrderByDescending(x => x.PostHistories.Count).First().PostHistories.Count : 0;
                var maxComments = helperTDO.PostCount > 0 ? _context.Posts.OrderByDescending(x => x.Comments.Count).First().Comments.Count : 0;
                var maxChildPosts = helperTDO.PostCount > 0 ? _context.Posts.OrderByDescending(x => x.ChildPosts.Count).First().ChildPosts.Count : 0;

                await _logger.LogActivity(
                    site.Id,
                    ActivityCodeEnum.PostList,
                    ActivitySeverityEnum.Information,
                    ActivityObjectEnum.Post,
                    "A list of all posts has been requested and recieved by user");

                return new PostsEnvelope
                {
                    Posts = _mapper.Map<List<PostDto>>(helperTDO.PostsEnvelope),
                    PostCount = helperTDO.PostCount,
                    MaxAttachmentsNumber = maxAttachmentsNumber,
                    MaxViewCount = maxViewCount,
                    MaxPostHistories = maxPostHistories,
                    MaxComments = maxComments,
                    MaxChildPosts = maxChildPosts
                };

            }
        }
    }
}