using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Errors;
using Aspian.Application.Core.Interfaces;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.SiteModel;
using Aspian.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.OptionServices.AdminServices
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
            private readonly IActivityLogger _logger;
            public Handler(DataContext context, IActivityLogger logger)
            {
                _logger = logger;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var site = await _context.Sites.FirstOrDefaultAsync(x => x.SiteType == SiteTypeEnum.Blog);
                
                foreach (var id in request.Ids)
                {
                    var optionmeta = await _context.Optionmetas.FindAsync(id);
                    if (optionmeta == null)
                        throw new RestException(HttpStatusCode.NotFound, new { optionmeta = "Not found!" });

                    optionmeta.Value = optionmeta.DefaultValue;
                }

                var success = await _context.SaveChangesAsync() > 0;

                if (success) 
                {
                    await _logger.LogActivity(
                        site.Id,
                        ActivityCodeEnum.OptionRestoreDefaultOptions,
                        ActivitySeverityEnum.Critical,
                        ActivityObjectEnum.Option,
                        $"All options has been restored to default.");

                    return Unit.Value;
                }

                throw new Exception("Problem saving changes!");
            }
        }
    }
}