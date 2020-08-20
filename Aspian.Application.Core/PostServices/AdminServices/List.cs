using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Interfaces;
using Aspian.Application.Core.PostServices.AdminServices.DTOs;
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

                var queryable = _context.Posts.AsQueryable();
                var postCount = queryable.Count();

                List<Post> posts;

                if (request.Field != null && request.Order != null && request.Order != "undefined")
                {
                    switch (request.Field)
                    {
                        case "title":
                            switch (request.Order)
                            {
                                case "ascend":
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderBy(x => x.Title)
                                        .ToListAsync();
                                    break;

                                case "descend":
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderByDescending(x => x.Title)
                                        .ToListAsync();
                                    break;

                                default:
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderByDescending(x => x.CreatedAt)
                                        .ThenByDescending(x => x.ModifiedAt)
                                        .ThenByDescending(x => x.Title)
                                        .ToListAsync();
                                    break;
                            }
                            break;

                        case "postCategory":
                            switch (request.Order)
                            {
                                case "ascend":
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderBy(x => x.TaxonomyPosts.FirstOrDefault(x => x.Taxonomy.Type == TaxonomyTypeEnum.category))
                                        .ToListAsync();
                                    break;

                                case "descend":
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderByDescending(x => x.TaxonomyPosts.FirstOrDefault(x => x.Taxonomy.Type == TaxonomyTypeEnum.category))
                                        .ToListAsync();
                                    break;

                                default:
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderByDescending(x => x.CreatedAt)
                                        .ThenByDescending(x => x.ModifiedAt)
                                        .ThenByDescending(x => x.Title)
                                        .ToListAsync();
                                    break;
                            }
                            break;

                        case "postStatus":
                            switch (request.Order)
                            {
                                case "ascend":
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderBy(x => x.PostStatus)
                                        .ToListAsync();
                                    break;

                                case "descend":
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderByDescending(x => x.PostStatus)
                                        .ToListAsync();
                                    break;

                                default:
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderByDescending(x => x.CreatedAt)
                                        .ThenByDescending(x => x.ModifiedAt)
                                        .ThenByDescending(x => x.Title)
                                        .ToListAsync();
                                    break;
                            }
                            break;

                        case "postAttachments":
                            switch (request.Order)
                            {
                                case "ascend":
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderBy(x => x.PostAttachments.Count)
                                        .ToListAsync();
                                    break;

                                case "descend":
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderByDescending(x => x.PostAttachments.Count)
                                        .ToListAsync();
                                    break;

                                default:
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderByDescending(x => x.CreatedAt)
                                        .ThenByDescending(x => x.ModifiedAt)
                                        .ThenByDescending(x => x.Title)
                                        .ToListAsync();
                                    break;
                            }
                            break;

                        case "commentAllowed":
                            switch (request.Order)
                            {
                                case "ascend":
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderBy(x => x.CommentAllowed)
                                        .ToListAsync();
                                    break;

                                case "descend":
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderByDescending(x => x.CommentAllowed)
                                        .ToListAsync();
                                    break;

                                default:
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderByDescending(x => x.CreatedAt)
                                        .ThenByDescending(x => x.ModifiedAt)
                                        .ThenByDescending(x => x.Title)
                                        .ToListAsync();
                                    break;
                            }
                            break;

                        case "viewCount":
                            switch (request.Order)
                            {
                                case "ascend":
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderBy(x => x.ViewCount)
                                        .ToListAsync();
                                    break;

                                case "descend":
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderByDescending(x => x.ViewCount)
                                        .ToListAsync();
                                    break;

                                default:
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderByDescending(x => x.CreatedAt)
                                        .ThenByDescending(x => x.ModifiedAt)
                                        .ThenByDescending(x => x.Title)
                                        .ToListAsync();
                                    break;
                            }
                            break;

                        case "postHistories":
                            switch (request.Order)
                            {
                                case "ascend":
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderBy(x => x.PostHistories.Count)
                                        .ToListAsync();
                                    break;

                                case "descend":
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderByDescending(x => x.PostHistories.Count)
                                        .ToListAsync();
                                    break;

                                default:
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderByDescending(x => x.CreatedAt)
                                        .ThenByDescending(x => x.ModifiedAt)
                                        .ThenByDescending(x => x.Title)
                                        .ToListAsync();
                                    break;
                            }
                            break;

                        case "comments":
                            switch (request.Order)
                            {
                                case "ascend":
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderBy(x => x.Comments.Count)
                                        .ToListAsync();
                                    break;

                                case "descend":
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderByDescending(x => x.Comments.Count)
                                        .ToListAsync();
                                    break;

                                default:
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderByDescending(x => x.CreatedAt)
                                        .ThenByDescending(x => x.ModifiedAt)
                                        .ThenByDescending(x => x.Title)
                                        .ToListAsync();
                                    break;
                            }
                            break;

                        case "childPosts":
                            switch (request.Order)
                            {
                                case "ascend":
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderBy(x => x.ChildPosts.Count)
                                        .ToListAsync();
                                    break;

                                case "descend":
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderByDescending(x => x.ChildPosts.Count)
                                        .ToListAsync();
                                    break;

                                default:
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderByDescending(x => x.CreatedAt)
                                        .ThenByDescending(x => x.ModifiedAt)
                                        .ThenByDescending(x => x.Title)
                                        .ToListAsync();
                                    break;
                            }
                            break;

                        case "createdAt":
                            switch (request.Order)
                            {
                                case "ascend":
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderBy(x => x.CreatedAt)
                                        .ToListAsync();
                                    break;

                                case "descend":
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderByDescending(x => x.CreatedAt)
                                        .ToListAsync();
                                    break;

                                default:
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderByDescending(x => x.CreatedAt)
                                        .ThenByDescending(x => x.ModifiedAt)
                                        .ThenByDescending(x => x.Title)
                                        .ToListAsync();
                                    break;
                            }
                            break;

                        case "createdBy":
                            switch (request.Order)
                            {
                                case "ascend":
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderBy(x => x.CreatedBy)
                                        .ToListAsync();
                                    break;

                                case "descend":
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderByDescending(x => x.CreatedBy)
                                        .ToListAsync();
                                    break;

                                default:
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderByDescending(x => x.CreatedAt)
                                        .ThenByDescending(x => x.ModifiedAt)
                                        .ThenByDescending(x => x.Title)
                                        .ToListAsync();
                                    break;
                            }
                            break;

                        case "modifiedAt":
                            switch (request.Order)
                            {
                                case "ascend":
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderBy(x => x.ModifiedAt)
                                        .ToListAsync();
                                    break;

                                case "descend":
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderByDescending(x => x.ModifiedAt)
                                        .ToListAsync();
                                    break;

                                default:
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderByDescending(x => x.CreatedAt)
                                        .ThenByDescending(x => x.ModifiedAt)
                                        .ThenByDescending(x => x.Title)
                                        .ToListAsync();
                                    break;
                            }
                            break;

                        case "modifiedBy":
                            switch (request.Order)
                            {
                                case "ascend":
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderBy(x => x.ModifiedBy)
                                        .ToListAsync();
                                    break;

                                case "descend":
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderByDescending(x => x.ModifiedBy)
                                        .ToListAsync();
                                    break;

                                default:
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderByDescending(x => x.CreatedAt)
                                        .ThenByDescending(x => x.ModifiedAt)
                                        .ThenByDescending(x => x.Title)
                                        .ToListAsync();
                                    break;
                            }
                            break;

                        case "userAgent":
                            switch (request.Order)
                            {
                                case "ascend":
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderBy(x => x.UserAgent)
                                        .ToListAsync();
                                    break;

                                case "descend":
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderByDescending(x => x.UserAgent)
                                        .ToListAsync();
                                    break;

                                default:
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderByDescending(x => x.CreatedAt)
                                        .ThenByDescending(x => x.ModifiedAt)
                                        .ThenByDescending(x => x.Title)
                                        .ToListAsync();
                                    break;
                            }
                            break;

                        case "userIPAddress":
                            switch (request.Order)
                            {
                                case "ascend":
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderBy(x => x.UserIPAddress)
                                        .ToListAsync();
                                    break;

                                case "descend":
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderByDescending(x => x.UserIPAddress)
                                        .ToListAsync();
                                    break;

                                default:
                                    posts =
                                        await queryable
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderByDescending(x => x.CreatedAt)
                                        .ThenByDescending(x => x.ModifiedAt)
                                        .ThenByDescending(x => x.Title)
                                        .ToListAsync();
                                    break;
                            }
                            break;

                        default:
                            posts =
                            await queryable
                            .Skip(request.Offset ?? 0)
                            .Take(request.Limit ?? 3)
                            .OrderByDescending(x => x.CreatedAt)
                            .ThenByDescending(x => x.ModifiedAt)
                            .ThenByDescending(x => x.Title)
                            .ToListAsync();
                            break;
                    }
                }
                else if (request.FilterKey != null && request.FilterValue != null)
                {
                    switch (request.FilterKey)
                    {
                        case "title":
                            switch (request.Order)
                            {
                                case "ascend":
                                    posts =
                                        await queryable
                                        .Where(x => x.Title.ToLower().Contains(request.FilterValue.ToLower()))
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderBy(x => x.Title)
                                        .ToListAsync();

                                    postCount = posts.Count;

                                    break;

                                case "descend":
                                    posts =
                                        await queryable
                                        .Where(x => x.Title.ToLower().Contains(request.FilterValue.ToLower()))
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderByDescending(x => x.Title)
                                        .ToListAsync();

                                    postCount = posts.Count;

                                    break;

                                default:
                                    posts =
                                        await queryable
                                        .Where(x => x.Title.ToLower().Contains(request.FilterValue.ToLower()))
                                        .Skip(request.Offset ?? 0)
                                        .Take(request.Limit ?? 3)
                                        .OrderByDescending(x => x.Title)
                                        .ToListAsync();

                                    postCount = posts.Count;

                                    break;
                            }
                            break;

                        case "postCategory":
                            switch (request.Order)
                            {
                                case "ascend":
                                    posts = await (from p in _context.Posts
                                                   from tp in p.TaxonomyPosts.Where(
                                                        tp => tp.Taxonomy.Type == TaxonomyTypeEnum.category &&
                                                        tp.Taxonomy.Term.Name.ToLower().Contains(request.FilterValue.ToLower()))
                                                   select p).ToListAsync();

                                    postCount = posts.Count;

                                    break;

                                case "descend":
                                    posts = await (from p in _context.Posts
                                                   from tp in p.TaxonomyPosts.Where(
                                                        tp => tp.Taxonomy.Type == TaxonomyTypeEnum.category &&
                                                        tp.Taxonomy.Term.Name.ToLower().Contains(request.FilterValue.ToLower()))
                                                   select p).ToListAsync();

                                    postCount = posts.Count;

                                    break;

                                default:
                                    posts = await (from p in _context.Posts
                                                   from tp in p.TaxonomyPosts.Where(
                                                        tp => tp.Taxonomy.Type == TaxonomyTypeEnum.category &&
                                                        tp.Taxonomy.Term.Name.ToLower().Contains(request.FilterValue.ToLower()))
                                                   select p).ToListAsync();

                                    postCount = posts.Count;

                                    break;
                            }
                            break;

                        case "userIPAddress":
                            switch (request.Order)
                            {
                                case "ascend":
                                    posts = await queryable.Where(x => x.UserIPAddress == request.FilterValue).ToListAsync();

                                    postCount = posts.Count;

                                    break;

                                case "descend":
                                    posts = await queryable.Where(x => x.UserIPAddress == request.FilterValue).ToListAsync();

                                    postCount = posts.Count;

                                    break;

                                default:
                                    posts = await queryable.Where(x => x.UserIPAddress == request.FilterValue).ToListAsync();

                                    postCount = posts.Count;

                                    break;
                            }
                            break;

                        default:
                            posts =
                            await queryable
                            .Skip(request.Offset ?? 0)
                            .Take(request.Limit ?? 3)
                            .OrderByDescending(x => x.CreatedAt)
                            .ThenByDescending(x => x.ModifiedAt)
                            .ThenByDescending(x => x.Title)
                            .ToListAsync();
                            break;
                    }
                }
                else
                {
                    posts =
                            await queryable
                            .Skip(request.Offset ?? 0)
                            .Take(request.Limit ?? 3)
                            .OrderByDescending(x => x.CreatedAt)
                            .ThenByDescending(x => x.ModifiedAt)
                            .ThenByDescending(x => x.Title)
                            .ToListAsync();
                }

                await _logger.LogActivity(
                    site.Id,
                    ActivityCodeEnum.PostList,
                    ActivitySeverityEnum.Information,
                    ActivityObjectEnum.Post,
                    "A list of all posts has been requested and recieved by user");

                return new PostsEnvelope
                {
                    Posts = _mapper.Map<List<PostDto>>(posts),
                    PostCount = postCount
                };

            }
        }
    }
}