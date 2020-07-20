using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Errors;
using Aspian.Persistence;
using MediatR;

namespace Aspian.Application.Core.OptionServices
{
    public class RestoreDefaultOption
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var optionmeta = await _context.Optionmetas.FindAsync(request.Id);
                if (optionmeta == null)
                    throw new RestException(HttpStatusCode.NotFound, new {optionmeta = "Not found!"});

                optionmeta.Value = optionmeta.DefaultValue;

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes!");
            }
        }
    }
}