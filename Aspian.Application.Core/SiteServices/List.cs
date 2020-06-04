using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.SiteServices.DTOs;
using Aspian.Domain.SiteModel;
using Aspian.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.SiteServices
{
    public class List
    {
        public class Query : IRequest<List<SiteDto>> { }

        public class Handler : IRequestHandler<Query, List<SiteDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<List<SiteDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var Sites = await _context.Sites.ToListAsync();

                return _mapper.Map<List<Site>, List<SiteDto>>(Sites);
            }
        }
    }
}