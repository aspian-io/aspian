using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.CommentServices.DTOs;
using Aspian.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.CommentServices
{
    public class List
    {
        public class Query : IRequest<List<CommentDto>> { }

        public class Handler : IRequestHandler<Query, List<CommentDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<List<CommentDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var comments = await _context.Comments.ToListAsync();

                return _mapper.Map<List<CommentDto>>(comments);
            }
        }
    }
}