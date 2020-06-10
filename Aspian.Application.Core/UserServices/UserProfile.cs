using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.UserServices.DTOs;
using Aspian.Domain.AttachmentModel;
using Aspian.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.UserServices
{
    public class UserProfile
    {
        public class Query : IRequest<UserProfileDto>
        {
            public string UserName { get; set; }
        }

        public class Handler : IRequestHandler<Query, UserProfileDto>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<UserProfileDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == request.UserName);

                return _mapper.Map<UserProfileDto>(user);

            }
        }
    }
}