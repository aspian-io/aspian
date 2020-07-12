using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.PostServices.DTOs;
using Aspian.Domain.PostModel;
using Aspian.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.PostServices
{
    public class List
    {
        public class Query : IRequest<List<PostDto>> { }

        public class Handler : IRequestHandler<Query, List<PostDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<List<PostDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var posts = await _context.Posts.ToListAsync();

                return _mapper.Map<List<PostDto>>(posts);
            }
        }
    }
}