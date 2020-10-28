using System;
using System.Linq;
using System.Net;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Errors;
using Aspian.Application.Core.Interfaces;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.BaseModel;
using Aspian.Domain.SiteModel;
using Aspian.Domain.TaxonomyModel;
using Aspian.Persistence;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.TaxonomyServices.AdminServices
{
    public class Create
    {
        public class Command : Entitymeta, IRequest
        {
            public TaxonomyTypeEnum Type { get; set; }
            public string Description { get; set; }


            public Guid? ParentId { get; set; }
            public Term Term { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Type).NotEmpty();
                RuleFor(x => x.Term.Name).NotEmpty().MaximumLength(150).WithName("Term name");
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly ISlugGenerator _slugGenerator;
            private readonly UrlEncoder _urlEncoder;
            private readonly IActivityLogger _logger;
            public Handler(DataContext context, IMapper mapper, ISlugGenerator slugGenerator, UrlEncoder urlEncoder, IActivityLogger logger)
            {
                _logger = logger;
                _urlEncoder = urlEncoder;
                _slugGenerator = slugGenerator;
                _mapper = mapper;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var site = await _context.Sites.SingleOrDefaultAsync(x => x.SiteType == SiteTypeEnum.Blog);
                request.Term.Slug = string.IsNullOrWhiteSpace(request.Term.Slug) ?
                                    _slugGenerator.GenerateSlug(request.Term.Name) :
                                    _urlEncoder.Encode(request.Term.Slug.Trim().Replace(" ", "-"));
                var taxonomy = _mapper.Map<Create.Command, Taxonomy>(request);
                taxonomy.SiteId = site.Id;

                if (await _context.Terms.Where(x => x.Name == taxonomy.Term.Name).AnyAsync())
                    throw new RestException(HttpStatusCode.BadRequest, new { TermName = "Term name is already exists. Please change it and try again." });

                if (await _context.Terms.Where(x => x.Slug == taxonomy.Term.Slug).AnyAsync())
                    throw new RestException(HttpStatusCode.BadRequest, new { TermSlug = "Term slug is already exists. Please change it and try again." });

                _context.Taxonomies.Add(taxonomy);

                var success = await _context.SaveChangesAsync() > 0;

                if (success)
                {
                    await _logger.LogActivity(
                        site.Id,
                        ActivityCodeEnum.TaxonomyCreate,
                        ActivitySeverityEnum.Low,
                        ActivityObjectEnum.Taxonomy,
                        $"The taxonomy with type of \"{taxonomy.Type.ToString()}\" and the term name \"{taxonomy.Term.Name}\" has been created.");

                    return Unit.Value;
                }

                throw new Exception("Problem saving changes");
            }
        }
    }
}