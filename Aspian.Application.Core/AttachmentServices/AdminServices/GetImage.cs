using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.AttachmentServices.AdminServices.DTOs;
using Aspian.Application.Core.Errors;
using Aspian.Application.Core.Interfaces;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.AttachmentModel;
using Aspian.Domain.SiteModel;
using Aspian.Domain.UserModel;
using Aspian.Domain.UserModel.Policy;
using Aspian.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.AttachmentServices.AdminServices
{
    public class GetImage
    {
        public class Query : IRequest<GetImageDto>
        {
            public string FileName { get; set; }
        }

        public class Handler : IRequestHandler<Query, GetImageDto>
        {
            private readonly DataContext _context;
            private readonly IUploadAccessor _uploadAccessor;
            private readonly IUserAccessor _userAccessor;
            private readonly UserManager<User> _userManager;
            public Handler(DataContext context, IUploadAccessor uploadAccessor, IUserAccessor userAccessor, UserManager<User> userManager)
            {
                _userManager = userManager;
                _userAccessor = userAccessor;
                _uploadAccessor = uploadAccessor;
                _context = context;
            }

            public async Task<GetImageDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var site = await _context.Sites.FirstOrDefaultAsync(x => x.SiteType == SiteTypeEnum.Blog);
                var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());
                var userClaims = await _userManager.GetClaimsAsync(user);

                var image = await _context.Attachments.SingleOrDefaultAsync(x => x.FileName == request.FileName && x.Type == AttachmentTypeEnum.Photo);
                if (image == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Photo = "Not found" });

                var imageMemoryStream = await _uploadAccessor.GetImageAsync(image.RelativePath, image.UploadLocation);

                return new GetImageDto
                {
                    FileName = image.FileName,
                    Memory = imageMemoryStream,
                    MimeType = image.MimeType
                };
            }
        }
    }
}