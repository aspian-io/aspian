using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Interfaces;
using Aspian.Domain.AttachmentModel;
using Aspian.Domain.PostModel;
using Aspian.Domain.TaxonomyModel;
using Aspian.Persistence;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Aspian.Application.Core.PostServices
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
            public bool CommentAllowed { get; set; }
            public int Order { get; set; }
            public int ViewCount { get; set; }
            public PostTypeEnum Type { get; set; }


            public virtual ICollection<Attachment> Attachments { get; set; }
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
            private readonly UrlEncoder _urlEncoder;
            public Handler(DataContext context, IMapper mapper, ISlugGenerator slugGenerator, UrlEncoder urlEncoder)
            {
                _urlEncoder = urlEncoder;
                _slugGenerator = slugGenerator;
                _mapper = mapper;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {



                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes!");
            }
        }
    }
}