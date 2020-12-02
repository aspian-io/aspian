using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.AttachmentServices.AdminServices.DTOs;
using Aspian.Application.Core.Errors;
using Aspian.Persistence;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.AttachmentServices.AdminServices
{
    public class FileBrowserFileDetails
    {
        public class Query : IRequest<FileBrowserDto>
        {
            public string FileName { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.FileName).NotEmpty().MaximumLength(60);
            }
        }

        public class Handler : IRequestHandler<Query, FileBrowserDto>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<FileBrowserDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var attachment = await _context.Attachments.SingleOrDefaultAsync(x => x.FileName == request.FileName);
                if (attachment == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { file = "not found!" });
                }
                return _mapper.Map<FileBrowserDto>(attachment);
            }
        }
    }
}