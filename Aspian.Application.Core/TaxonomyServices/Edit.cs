using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Errors;
using Aspian.Domain.BaseModel;
using Aspian.Domain.TaxonomyModel;
using Aspian.Persistence;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Aspian.Application.Core.TaxonomyServices
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
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var taxonomy = await _context.TermTaxonomies.FindAsync(request.Id);

                if (taxonomy == null)
                    throw new RestException(HttpStatusCode.NotFound, new { taxonomy = "Not found!" });

                _mapper.Map(request, taxonomy);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes!");
            }
        }
    }
}