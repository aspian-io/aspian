using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.PostServices.AdminServices.DTOs;
using Aspian.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.PostServices.AdminServices
{
    public class ParentPostTreeSelectAntd
    {
        public class Query : IRequest<List<AntParentPostTreeSelectDto>> { }

        public class Handler : IRequestHandler<Query, List<AntParentPostTreeSelectDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<List<AntParentPostTreeSelectDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var posts = await _context.Posts.ToListAsync();
                return _mapper.Map<List<AntParentPostTreeSelectDto>>(posts);
            }
        }
    }
}