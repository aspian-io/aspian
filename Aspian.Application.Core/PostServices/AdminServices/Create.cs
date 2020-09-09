using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Errors;
using Aspian.Application.Core.Interfaces;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.PostModel;
using Aspian.Domain.ScheduleModel;
using Aspian.Domain.SiteModel;
using Aspian.Domain.TaxonomyModel;
using Aspian.Persistence;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.PostServices.AdminServices
{
    public class Create
    {
        public class Command : IRequest
        {
            public string Title { get; set; }
            public string Subtitle { get; set; }
            public string Excerpt { get; set; }
            public string Content { get; set; }
            public string Slug { get; set; }
            public PostStatusEnum PostStatus { get; set; }
            public string ScheduledFor { get; set; }
            public bool CommentAllowed { get; set; }
            public int Order { get; set; }
            public int ViewCount { get; set; }
            public PostTypeEnum Type { get; set; }
            public bool IsPinned { get; set; }
            public int PinOrder { get; set; }


            public virtual ICollection<PostAttachment> PostAttachments { get; set; }
            public Guid? ParentId { get; set; }
            public virtual ICollection<TaxonomyPost> TaxonomyPosts { get; set; }
            public virtual ICollection<Postmeta> Postmetas { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Title).NotEmpty().MaximumLength(150);
                RuleFor(x => x.Content).NotEmpty();
                RuleFor(x => x.PostStatus).NotNull();
                RuleFor(x => x.Type).NotNull();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly ISlugGenerator _slugGenerator;
            private readonly IActivityLogger _logger;
            private readonly IScheduler _scheduler;
            public Handler(DataContext context, IMapper mapper, ISlugGenerator slugGenerator, IScheduler scheduler, IActivityLogger logger)
            {
                _scheduler = scheduler;
                _logger = logger;
                _slugGenerator = slugGenerator;
                _mapper = mapper;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var isTitleExist = await _context.Posts.SingleOrDefaultAsync(x => x.Title == request.Title) != null;
                if (isTitleExist)
                    throw new RestException(HttpStatusCode.BadRequest, new { title = "duplicate title is no allowed" });

                var site = await _context.Sites.SingleOrDefaultAsync(x => x.SiteType == SiteTypeEnum.Blog);

                request.Slug = string.IsNullOrWhiteSpace(request.Slug) ?
                                _slugGenerator.GenerateSlug(request.Title) :
                                _slugGenerator.GenerateSlug(request.Slug);

                var post = _mapper.Map<Post>(request);
                post.Site = site;
                post.ScheduledFor = request.PostStatus == PostStatusEnum.Future ? Convert.ToDateTime(request.ScheduledFor) : DateTime.UtcNow;

                _context.Posts.Add(post);

                var success = await _context.SaveChangesAsync() > 0;

                if (request.PostStatus == PostStatusEnum.Future && post.ScheduledFor != null && post.Id != null)
                {
                    await _scheduler.SetAndQueueTaskAsync(ScheduleTypeEnum.ScheduledPost, post.ScheduledFor ?? DateTime.UtcNow, post.Id);
                } else {
                    throw new RestException(HttpStatusCode.BadRequest, new { post = "publish date is required!" });
                }

                if (success)
                {
                    var postTruncatedContent = post.Title.Length > 30 ? post.Title.Substring(0, 30) + "..." : post.Title;
                    var postAuthorUsername = post.CreatedBy.UserName;
                    await _logger.LogActivity(
                        site.Id,
                        ActivityCodeEnum.PostCreate,
                        ActivitySeverityEnum.Low,
                        ActivityObjectEnum.Post,
                        $"The post \"{postTruncatedContent}\" written by the user \"{postAuthorUsername}\" has been created.");

                    return Unit.Value;
                }

                throw new Exception("Problem saving changes!");
            }
        }
    }
}