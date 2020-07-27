using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Errors;
using Aspian.Domain.SiteModel;
using Aspian.Persistence;
using AutoMapper;
using MediatR;

namespace Aspian.Application.Core.SiteServices.AdminServices
{
    public class DeveloperEdit
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
            public string Domain { get; set; }
            public string Path { get; set; }
            public SiteTypeEnum SiteType { get; set; }
            public bool IsActivated { get; set; }
            public DateTime? ModuleActivatedAt { get; set; }
            public DateTime? ModuleExpiresAt { get; set; }
            public DateTime? MainHostActivatedAt { get; set; }
            public DateTime? MainHostExpiresAt { get; set; }
            public long MainHostCapacity { get; set; }
            public long MainHostAvailableSpace { get; set; }
            public bool HasDownloadHost { get; set; }
            public DateTime? DownloadHostActivatedAt { get; set; }
            public DateTime? DownloadHostExpiresAt { get; set; }
            public long? DownloadHostCapacity { get; set; }
            public long? DownloadHostAvailableSpace { get; set; }
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
                var site = await _context.Sites.FindAsync(request.Id);

                if (site == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Site = "not found!" });

                _mapper.Map(request, site);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes!");
            }
        }
    }
}