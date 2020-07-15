using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.CommentServices.DTOs;
using Aspian.Application.Core.Errors;
using Aspian.Persistence;
using AutoMapper;
using MediatR;

namespace Aspian.Application.Core.CommentServices
{
    public class Details
    {
        public class Query : IRequest<CommentDto>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, CommentDto>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<CommentDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var comment = await _context.Comments.FindAsync(request.Id);
                if (comment == null)
                    throw new RestException(HttpStatusCode.NotFound, new { comment = "Not found!" });

                return _mapper.Map<CommentDto>(comment);
            }
        }
    }
}