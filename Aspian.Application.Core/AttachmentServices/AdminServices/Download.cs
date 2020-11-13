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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.AttachmentServices.AdminServices
{
    public class Download
    {
        public class Query : IRequest<DownloadFileDto>
        {
            public string FileName { get; set; }
        }

        public class Handler : IRequestHandler<Query, DownloadFileDto>
        {
            private readonly DataContext _context;
            private readonly IUploadAccessor _uploadAccessor;
            private readonly IActivityLogger _logger;
            private readonly IHttpContextAccessor _httpContextAccessor;
            public Handler(DataContext context, IUploadAccessor uploadAccessor, IHttpContextAccessor httpContextAccessor, IActivityLogger logger)
            {
                _httpContextAccessor = httpContextAccessor;
                _logger = logger;
                _uploadAccessor = uploadAccessor;
                _context = context;
            }

            public async Task<DownloadFileDto> Handle(Query request, CancellationToken cancellationToken)
            {
                // var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
                // if (refreshToken == null)
                //     throw new RestException(HttpStatusCode.Unauthorized, new { User = "is unathorized!" });

                var site = await _context.Sites.FirstOrDefaultAsync(x => x.SiteType == SiteTypeEnum.Blog);
                var file = _context.Attachments.SingleOrDefault(x => x.FileName == Path.GetFileNameWithoutExtension(request.FileName));
                if (file == null)
                    throw new RestException(HttpStatusCode.NotFound, new { File = "Not found" });

                await _logger.LogActivity(
                        site.Id,
                        ActivityCodeEnum.AttachmentDownload,
                        ActivitySeverityEnum.Information,
                        ActivityObjectEnum.Attachemnt,
                        $"The {file.Type} with the name {file.FileName} downloaded");

                return new DownloadFileDto
                {
                    FileName = file.PublicFileName,
                    Stream = await _uploadAccessor.DownloadFileAsync(file.RelativePath, file.FileSize, file.UploadLocation),
                    MimeType = file.MimeType
                };
            }
        }
    }
}