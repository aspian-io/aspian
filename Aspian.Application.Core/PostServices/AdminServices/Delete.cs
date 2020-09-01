using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Errors;
using Aspian.Application.Core.Interfaces;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.PostModel;
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
            public List<Guid> Ids { get; set; }
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
                var posts = new List<Post>();

                foreach (var id in request.Ids)
                {
                    var post = await _context.Posts.Where(x => x.Type == PostTypeEnum.Posts).SingleOrDefaultAsync(x => x.Id == id);

                    if (post == null)
                        throw new RestException(HttpStatusCode.NotFound, new { post = "not found" });

                    _context.PostAttachments.RemoveRange(post.PostAttachments);
                    _context.TaxonomyPosts.RemoveRange(post.TaxonomyPosts);
                    foreach (var item in post.Comments)
                    {
                        await RemoveCommentChildren(item.Id);
                    }
                    _context.Comments.RemoveRange(post.Comments);
                    await RemovePostChildren(post.Id);
                    _context.Posts.Remove(post);

                    posts.Add(post);
                }


                var success = await _context.SaveChangesAsync() > 0;

                if (success)
                {
                    foreach (var post in posts)
                    {
                        var postTruncatedContent = post.Title.Length > 30 ? post.Title.Substring(0, 30) + "..." : post.Title;

                        await _logger.LogActivity(
                        site.Id,
                        ActivityCodeEnum.CommentDelete,
                        ActivitySeverityEnum.Low,
                        ActivityObjectEnum.Comment,
                        $"The post \"{postTruncatedContent}\" has been deleted.");
                    }

                    return Unit.Value;
                }

                throw new Exception("Problem saving changes!");
            }

            // Removing post children recursively
            private async Task RemovePostChildren(Guid id)
            {
                var children = await _context.Posts.Where(c => c.ParentId == id).ToListAsync();
                foreach (var child in children)
                {
                    await RemovePostChildren(child.Id);
                    _context.Posts.Remove(child);
                }
            }

            // Removing comment children recursively
            private async Task RemoveCommentChildren(Guid id)
            {
                var children = await _context.Comments.Where(c => c.ParentId == id).ToListAsync();
                foreach (var child in children)
                {
                    await RemoveCommentChildren(child.Id);
                    _context.Comments.Remove(child);
                }
            }
        }
    }
}