using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Errors;
using Aspian.Application.Core.Interfaces;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.SiteModel;
using Aspian.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.PostServices.AdminServices
{
    public class Delete
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IActivityLogger _logger;
            public Handler(DataContext context, IActivityLogger logger)
            {
                _logger = logger;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var site = await _context.Sites.FirstOrDefaultAsync(x => x.SiteType == SiteTypeEnum.Blog);
                var post = await _context.Posts.FindAsync(request.Id);
                if (post == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { post = "not found" });

                var postTruncatedContent = post.Title.Substring(0, 30) + "...";
                var postAuthorUsername = post.CreatedBy.UserName;

                _context.PostAttachments.RemoveRange(post.PostAttachments);
                _context.TaxonomyPosts.RemoveRange(post.TaxonomyPosts);
                _context.Posts.Remove(post);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) 
                {
                    await _logger.LogActivity(
                        site.Id,
                        ActivityCodeEnum.CommentDelete,
                        ActivitySeverityEnum.Low,
                        ActivityObjectEnum.Comment,
                        $"The post \"{postTruncatedContent}\" written by the user \"{postAuthorUsername}\" has been deleted.");

                    return Unit.Value;
                }

                throw new Exception("Problem saving changes!");
            }
        }
    }
}