using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Interfaces;
using Aspian.Application.Core.UserServices.AdminServices.DTOs;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.AttachmentModel;
using Aspian.Domain.SiteModel;
using Aspian.Domain.UserModel;
using Aspian.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.UserServices.AdminServices
{
    public class CurrentUser
    {
        public class Query : IRequest<CurrentUserDto> { }

        public class Handler : IRequestHandler<Query, CurrentUserDto>
        {
            private readonly UserManager<User> _userManager;
            private readonly IUserAccessor _userAccessor;
            public Handler(UserManager<User> userManager, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _userManager = userManager;

            }

            public async Task<CurrentUserDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());

                return new CurrentUserDto
                {
                    DisplayName = user.DisplayName,
                    UserName = user.UserName,
                    Role = user.Role,
                    ProfilePhotoName = user.CreatedAttachments?.FirstOrDefault(x => x.Type == AttachmentTypeEnum.Photo && x.IsMain).FileName
                };
            }
        }
    }
}