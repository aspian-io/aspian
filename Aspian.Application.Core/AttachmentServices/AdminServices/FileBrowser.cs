using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.AttachmentServices.AdminServices.DTOs;
using Aspian.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.AttachmentServices.AdminServices
{
    public class FileBrowser
    {
        public class Query : IRequest<List<FileBrowserDto>> { }

        public class Handler : IRequestHandler<Query, List<FileBrowserDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<List<FileBrowserDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var files = await _context.Attachments.ToListAsync();

                return _mapper.Map<List<FileBrowserDto>>(files);
            }
        }
    }
}