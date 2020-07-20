using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Errors;
using Aspian.Persistence;
using MediatR;

namespace Aspian.Application.Core.OptionServices
{
    public class RestoreDefaultOptions
    {
        public class Command : IRequest
        {
            public List<Guid> Ids { get; set; }
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
                foreach (var id in request.Ids)
                {
                    var optionmeta = await _context.Optionmetas.FindAsync(id);
                    if (optionmeta == null)
                        throw new RestException(HttpStatusCode.NotFound, new {optionmeta = "Not found!"});

                    optionmeta.Value = optionmeta.DefaultValue;
                }

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes!");
            }
        }
    }
}