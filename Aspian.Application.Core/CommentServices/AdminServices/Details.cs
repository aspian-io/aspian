using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.CommentServices.DTOs;
using Aspian.Application.Core.Errors;
using Aspian.Application.Core.Interfaces;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.SiteModel;
using Aspian.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.CommentServices.AdminServices
{
    public class Details
    {
        public class Query : IRequest<CommentDto>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, CommentDto>
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

            public async Task<CommentDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var site = await _context.Sites.FirstOrDefaultAsync(x => x.SiteType == SiteTypeEnum.Blog);
                var comment = await _context.Comments.FindAsync(request.Id);
                if (comment == null)
                    throw new RestException(HttpStatusCode.NotFound, new { comment = "Not found!" });

                await _logger.LogActivity(
                    site.Id,
                    ActivityCodeEnum.CommentDetails,
                    ActivitySeverityEnum.Information,
                    ActivityObjectEnum.Comment,
                    $"The comment \"{comment.Content.Substring(0, 30)}\" has been read.");

                return _mapper.Map<CommentDto>(comment);
            }
        }
    }
}