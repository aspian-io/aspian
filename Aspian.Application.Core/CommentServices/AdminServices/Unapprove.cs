using System;
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

namespace Aspian.Application.Core.CommentServices.AdminServices
{
    public class Unapprove
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
                var comment = await _context.Comments.FindAsync(request.Id);
                if (comment == null)
                    throw new RestException(HttpStatusCode.NotFound, new { comment = "Not found!" });

                if (!comment.Approved)
                    throw new RestException(HttpStatusCode.BadRequest, new {comment = "is already unapproved!"});

                comment.Approved = false;

                var success = await _context.SaveChangesAsync() > 0;

                if (success)
                {
                    await _logger.LogActivity(
                        site.Id,
                        ActivityCodeEnum.CommentApprove,
                        ActivitySeverityEnum.Information,
                        ActivityObjectEnum.Comment,
                        $"The comment \"{comment.Content.Substring(0, 30)}\" sent by the user \"{comment.CreatedBy.UserName}\" unapproved");

                    return Unit.Value;
                }

                throw new Exception("Problem saving changes!");
            }
        }
    }
}