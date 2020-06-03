using System;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Domain.BaseModel;
using Aspian.Domain.TaxonomyModel;
using Aspian.Persistence;
using AutoMapper;
using FluentValidation;
using MediatR;

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
                RuleFor(x => x.Term.Name).NotEmpty().WithName("Term name");
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
                var taxonomy = _mapper.Map<Create.Command, TermTaxonomy>(request);

                _context.TermTaxonomies.Add(taxonomy);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}