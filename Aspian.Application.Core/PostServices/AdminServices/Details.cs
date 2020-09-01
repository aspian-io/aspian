using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Errors;
using Aspian.Application.Core.Interfaces;
using Aspian.Application.Core.PostServices.AdminServices.DTOs;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.PostModel;
using Aspian.Domain.SiteModel;
using Aspian.Persistence;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.PostServices.AdminServices
{
    public class Details
    {
        public class Query : IRequest<PostDto>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, PostDto>
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

            public async Task<PostDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var site = await _context.Sites.FirstOrDefaultAsync(x => x.SiteType == SiteTypeEnum.Blog);
                var post = await _context.Posts.FindAsync(request.Id);

                if (post == null)
                    throw new RestException(HttpStatusCode.NotFound, new { post = "Not found!" });

                var postTitleSubstring = post.Title.Length > 30 ? post.Title.Substring(0, 30) + "..." : post.Title;
                await _logger.LogActivity(
                    site.Id,
                    ActivityCodeEnum.PostDetails,
                    ActivitySeverityEnum.Information,
                    ActivityObjectEnum.Post,
                    $"The post \"{postTitleSubstring}\" has been read.");

                return _mapper.Map<PostDto>(post);
            }
        }
    }
}