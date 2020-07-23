using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Errors;
using Aspian.Application.Core.Interfaces;
using Aspian.Application.Core.PostServices.DTOs;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.AttachmentModel;
using Aspian.Domain.PostModel;
using Aspian.Domain.SiteModel;
using Aspian.Domain.TaxonomyModel;
using Aspian.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.PostServices.AdminServices
{
    public class Edit
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Subtitle { get; set; }
            public string Excerpt { get; set; }
            public string Content { get; set; }
            public string Slug { get; set; }
            public PostStatusEnum PostStatus { get; set; }
            public bool CommentAllowed { get; set; }
            public int Order { get; set; }
            public int ViewCount { get; set; }
            public PostTypeEnum Type { get; set; }
            public bool IsPinned { get; set; }
            public int PinOrder { get; set; }


            public virtual ICollection<PostAttachmentDto> PostAttachments { get; set; }
            public Guid? ParentId { get; set; }
            public virtual ICollection<TaxonomyPostDto> TaxonomyPosts { get; set; }
            public virtual ICollection<PostmetaDto> Postmetas { get; set; }
        }

        public class Handler : IRequestHandler<Command>
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

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var site = await _context.Sites.FirstOrDefaultAsync(x => x.SiteType == SiteTypeEnum.Blog);
                var post = await _context.Posts.FindAsync(request.Id);
                if (post == null)
                    throw new RestException(HttpStatusCode.NotFound, new { post = "not found" });

                if (request.Title != post.Title)
                {
                    var isTitleExist = await _context.Posts.SingleOrDefaultAsync(x => x.Title == request.Title) != null;
                    if (isTitleExist)
                        throw new RestException(HttpStatusCode.BadRequest, new { title = "duplicate title is no allowed" });
                }

                if (request.Slug != post.Slug)
                {
                    var isSlugExist = await _context.Posts.SingleOrDefaultAsync(x => x.Slug == request.Slug) != null;
                    if (isSlugExist)
                        throw new RestException(HttpStatusCode.BadRequest, new { slug = "duplicate slug is no allowed" });
                }

                var postHistory = _mapper.Map<PostHistory>(post);
                postHistory.PostId = post.Id;
                _context.PostHistories.Add(postHistory);

                _mapper.Map(request, post);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) 
                {
                    await _logger.LogActivity(
                        site.Id,
                        ActivityCodeEnum.PostEdit,
                        ActivitySeverityEnum.Low,
                        ActivityObjectEnum.Post,
                        $"The comment \"{post.Title.Substring(0, 30)}...\" has been modified.");

                    return Unit.Value;
                }

                throw new Exception("Problem saving changes!");
            }
        }
    }
}