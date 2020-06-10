using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Interfaces;
using Aspian.Application.Core.AttachmentServices.DTOs;
using Aspian.Domain.AttachmentModel;
using Aspian.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.AttachmentServices
{
    public class Add
    {
        public class Command : IRequest<AttachmentDto>
        {
            public IFormFile File { get; set; }
        }

        public class Handler : IRequestHandler<Command, AttachmentDto>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly IUploadAccessor _uploadAccessor;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IUserAccessor userAccessor, IUploadAccessor uploadAccessor, IMapper mapper)
            {
                _mapper = mapper;
                _uploadAccessor = uploadAccessor;
                _userAccessor = userAccessor;
                _context = context;
            }

            public async Task<AttachmentDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var fileUploadResult = await _uploadAccessor.AddFileAsync(request.File);

                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUsername());

                if (!user.CreatedAttachments.Any(x => x.IsMain && x.Type == AttachmentTypeEnum.Photo))
                    fileUploadResult.IsMain = true;

                var userAttachments = _mapper.Map<FileUploadResult, Attachment>(fileUploadResult);

                user.CreatedAttachments.Add(userAttachments);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return _mapper.Map<AttachmentDto>(fileUploadResult);

                throw new Exception("Problem saving changes!");
            }
        }
    }
}