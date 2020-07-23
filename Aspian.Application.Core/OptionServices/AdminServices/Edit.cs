using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Errors;
using Aspian.Application.Core.Interfaces;
using Aspian.Application.Core.OptionServices.DTOs;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.OptionModel;
using Aspian.Domain.SiteModel;
using Aspian.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.OptionServices.AdminServices
{
    public class Edit
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
            public ValueEnum Value { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IActivityLogger _logger;
            public Handler(DataContext context, IActivityLogger logger)
            {
                _logger = logger;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var site = await _context.Sites.FirstOrDefaultAsync(x => x.SiteType == SiteTypeEnum.Blog);
                var optionmeta = await _context.Optionmetas.FindAsync(request.Id);
                if (optionmeta == null)
                    throw new RestException(HttpStatusCode.NotFound, new { optionmeta = "Not found!" });

                optionmeta.Value = request.Value;

                var success = await _context.SaveChangesAsync() > 0;

                if (success)
                {
                    await _logger.LogActivity(
                        site.Id,
                        ActivityCodeEnum.OptionEdit,
                        ActivitySeverityEnum.High,
                        ActivityObjectEnum.Option,
                        $"The option \"{optionmeta.KeyDescription}\" has been changed.");

                    return Unit.Value;
                }

                throw new Exception("Problem saving changes!");
            }
        }
    }
}