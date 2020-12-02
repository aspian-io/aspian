using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.AttachmentServices.AdminServices.DTOs;
using Aspian.Domain.AttachmentModel;
using Aspian.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.AttachmentServices.AdminServices
{
    public class FileBrowser
    {
        public class Query : IRequest<List<FileBrowserDto>>
        {
            public AttachmentTypeEnum? Type { get; set; }
        }

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
                List<Attachment> attachments;
                if (request.Type == null)
                {
                    attachments = await _context.Attachments.ToListAsync();
                }
                else
                {
                    if (request.Type == AttachmentTypeEnum.Video || request.Type == AttachmentTypeEnum.Photo)
                    {
                        attachments = await _context.Attachments.Where(a => a.Type == request.Type).ToListAsync();
                    }
                    else
                    {
                        attachments = await _context.Attachments
                            .Where(a => a.Type != AttachmentTypeEnum.Video && a.Type != AttachmentTypeEnum.Photo)
                            .ToListAsync();
                    }
                }

                return _mapper.Map<List<FileBrowserDto>>(attachments);
            }
        }
    }
}