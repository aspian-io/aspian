using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Interfaces;
using Aspian.Application.Core.AttachmentServices.AdminServices.DTOs;
using Aspian.Domain.AttachmentModel;
using Aspian.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Aspian.Domain.SiteModel;
using Aspian.Domain.ActivityModel;

namespace Aspian.Application.Core.AttachmentServices.AdminServices
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
            private readonly IActivityLogger _logger;
            public Handler(DataContext context, IUserAccessor userAccessor, IUploadAccessor uploadAccessor, IMapper mapper, IActivityLogger logger)
            {
                _logger = logger;
                _mapper = mapper;
                _uploadAccessor = uploadAccessor;
                _userAccessor = userAccessor;
                _context = context;
            }

            public async Task<AttachmentDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var site = await _context.Sites.FirstOrDefaultAsync(x => x.SiteType == SiteTypeEnum.Blog);

                var fileUploadResult = await _uploadAccessor.AddFileAsync(request.File);

                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUsername());

                if (!user.CreatedAttachments.Any(x => x.IsMain && x.Type == AttachmentTypeEnum.Photo))
                    fileUploadResult.IsMain = true;

                var userAttachment = _mapper.Map<FileUploadResult, Attachment>(fileUploadResult);
                userAttachment.Site = site;

                user.CreatedAttachments.Add(userAttachment);

                var success = await _context.SaveChangesAsync() > 0;

                if (success)
                {
                    await _logger.LogActivity(
                        site.Id, 
                        ActivityCodeEnum.AttachmentAdd, 
                        ActivitySeverityEnum.Medium, 
                        ActivityObjectEnum.Attachemnt, 
                        $"The {userAttachment.Type} with the name {userAttachment.FileName} uploaded");

                    return _mapper.Map<AttachmentDto>(userAttachment);
                } 

                throw new Exception("Problem saving changes!");
            }
        }
    }
}