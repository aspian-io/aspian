using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Interfaces;
using Aspian.Application.Core.UserServices.AdminServices.DTOs;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.AttachmentModel;
using Aspian.Domain.SiteModel;
using Aspian.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.UserServices.AdminServices
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
            private readonly IActivityLogger _logger;
            public Handler(DataContext context, IMapper mapper, IActivityLogger logger)
            {
                _logger = logger;
                _mapper = mapper;
                _context = context;
            }

            public async Task<UserProfileDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var site = await _context.Sites.FirstOrDefaultAsync(x => x.SiteType == SiteTypeEnum.Blog);
                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == request.UserName);

                var userProfileDto = _mapper.Map<UserProfileDto>(user);
                userProfileDto.Photo = _mapper.Map<PhotoDto>(user.CreatedAttachments.FirstOrDefault(x => x.Type == AttachmentTypeEnum.Photo && x.IsMain));

                await _logger.LogActivity(
                    site.Id,
                    ActivityCodeEnum.UserProfile,
                    ActivitySeverityEnum.Information,
                    ActivityObjectEnum.User,
                    $"A user profile with the username \"{user.UserName}\" has been reviewed.");

                return userProfileDto;

            }
        }
    }
}