using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Interfaces;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.CommentModel;
using Aspian.Domain.OptionModel;
using Aspian.Domain.SiteModel;
using Aspian.Persistence;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.CommentServices.AdminServices
{
    public class Create
    {
        public class Command : IRequest
        {
            public string Content { get; set; }

            public Guid? ParentId { get; set; }
            public Guid PostId { get; set; }
            public virtual ICollection<Commentmeta> Commentmetas { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Content).NotEmpty();
                RuleFor(x => x.PostId).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IOptionAccessor _optionAccessor;
            private readonly IActivityLogger _logger;
            public Handler(DataContext context, IMapper mapper, IOptionAccessor optionAccessor, IActivityLogger logger)
            {
                _logger = logger;
                _optionAccessor = optionAccessor;
                _mapper = mapper;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var site = await _context.Sites.FirstOrDefaultAsync(x => x.SiteType == SiteTypeEnum.Blog);
                var option = await _optionAccessor.GetOptionByKeyAsync(KeyEnum.Blog__Comment);

                var comment = _mapper.Map<Comment>(request);
                comment.Site = await _context.Sites.SingleOrDefaultAsync(x => x.SiteType == SiteTypeEnum.Blog);
                comment.Approved = option.Value == ValueEnum.Comment_Approved;

                _context.Comments.Add(comment);

                var success = await _context.SaveChangesAsync() > 0;

                if (success)
                {
                    await _logger.LogActivity(
                        site.Id,
                        ActivityCodeEnum.CommentCreate,
                        ActivitySeverityEnum.Low,
                        ActivityObjectEnum.Comment,
                        $"The comment \"{comment.Content.Substring(0, 30)}\" written by the user \"{comment.CreatedBy.UserName}\" has been sent.");

                    return Unit.Value;
                }

                throw new Exception("Problem saving changes!");
            }
        }
    }
}