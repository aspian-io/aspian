using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Domain.CommentModel;
using Aspian.Domain.OptionModel;
using Aspian.Domain.SiteModel;
using Aspian.Persistence;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.CommentServices
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
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var option = await _context.Options.SingleOrDefaultAsync(x => x.Section == SectionEnum.Comment);

                var comment = _mapper.Map<Comment>(request);
                comment.Site = await _context.Sites.SingleOrDefaultAsync(x => x.SiteType == SiteTypeEnum.Blog);
                comment.Approved = option.Optionmetas.SingleOrDefault(x => x.Key == KeyEnum.Comment_Blog).Value == ValueEnum.Comment_Approved;

                _context.Comments.Add(comment);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes!");
            }
        }
    }
}