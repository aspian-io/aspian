using System;
using System.Net;
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
    public class Edit
    {
        public class Command : Entitymeta, IRequest
        {
            public string Description { get; set; }
            public Guid? ParentId { get; set; }
            public Term Term { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Term.Name).NotEmpty().WithName("Term name");
            }
        }

        public class Handler : IRequestHandler<Command>
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

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var site = await _context.Sites.FirstOrDefaultAsync(x => x.SiteType == SiteTypeEnum.Blog);
                var taxonomy = await _context.Taxonomies.FindAsync(request.Id);

                if (taxonomy == null)
                    throw new RestException(HttpStatusCode.NotFound, new { taxonomy = "Not found!" });

                var formerTerm = taxonomy.Term.Name;

                _mapper.Map(request, taxonomy);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) 
                {
                    await _logger.LogActivity(
                        site.Id,
                        ActivityCodeEnum.TaxonomyEdit,
                        ActivitySeverityEnum.Medium,
                        ActivityObjectEnum.Taxonomy,
                        $"The taxonomy term with type of ({taxonomy.Type.ToString()}) and with the former name \"{formerTerm}\" changed to \"{taxonomy.Term.Name}\".");

                    return Unit.Value;
                }

                throw new Exception("Problem saving changes!");
            }
        }
    }
}