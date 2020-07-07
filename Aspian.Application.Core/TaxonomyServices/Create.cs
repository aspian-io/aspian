using System;
using System.Linq;
using System.Net;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Errors;
using Aspian.Application.Core.Interfaces;
using Aspian.Domain.BaseModel;
using Aspian.Domain.TaxonomyModel;
using Aspian.Persistence;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.TaxonomyServices
{
    public class Create
    {
        public class Command : Entitymeta, IRequest
        {
            public TaxonomyEnum Taxonomy { get; set; }
            public string Description { get; set; }


            public Guid? ParentId { get; set; }
            public Term Term { get; set; }
            public Guid SiteId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Taxonomy).NotEmpty();
                RuleFor(x => x.Term.Name).NotEmpty().MaximumLength(150).WithName("Term name");
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
                request.Term.Slug = string.IsNullOrWhiteSpace(request.Term.Slug) ?
                                    _slugGenerator.GenerateSlug(request.Term.Name) :
                                    _urlEncoder.Encode(request.Term.Slug.Trim().Replace(" ", "-"));
                var taxonomy = _mapper.Map<Create.Command, TermTaxonomy>(request);

                if (await _context.Terms.Where(x => x.Name == taxonomy.Term.Name).AnyAsync())
                    throw new RestException(HttpStatusCode.BadRequest, new { TermName = "Term name is already exists. Please change it and try again." });

                if (await _context.Terms.Where(x => x.Slug == taxonomy.Term.Slug).AnyAsync())
                    throw new RestException(HttpStatusCode.BadRequest, new { TermSlug = "Term slug is already exists. Please change it and try again." });

                _context.TermTaxonomies.Add(taxonomy);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}