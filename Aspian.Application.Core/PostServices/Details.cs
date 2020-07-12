using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Errors;
using Aspian.Application.Core.PostServices.DTOs;
using Aspian.Domain.PostModel;
using Aspian.Persistence;
using AutoMapper;
using MediatR;

namespace Aspian.Application.Core.PostServices
{
    public class Details
    {
        public class Query : IRequest<PostDto>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, PostDto>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<PostDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var post = await _context.Posts.FindAsync(request.Id);

                if (post == null)
                    throw new RestException(HttpStatusCode.NotFound, new { post = "Not found!" });

                return _mapper.Map<PostDto>(post);
            }
        }
    }
}