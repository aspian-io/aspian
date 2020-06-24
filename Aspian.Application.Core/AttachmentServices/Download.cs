using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.AttachmentServices.DTOs;
using Aspian.Application.Core.Errors;
using Aspian.Application.Core.Interfaces;
using Aspian.Domain.AttachmentModel;
using Aspian.Domain.UserModel;
using Aspian.Domain.UserModel.Policy;
using Aspian.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.AttachmentServices
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
            public Handler(DataContext context, IUploadAccessor uploadAccessor)
            {
                _uploadAccessor = uploadAccessor;
                _context = context;
            }

            public async Task<DownloadFileDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var file = _context.Attachments.SingleOrDefault(x => x.FileName == request.FileName && x.Type != AttachmentTypeEnum.Photo);
                if (file == null)
                    throw new RestException(HttpStatusCode.NotFound, new { File = "Not found" });


                var fileMemoryStream = await _uploadAccessor.DownloadFileAsync(file.RelativePath, file.UploadLocation);

                return new DownloadFileDto
                {
                    FileName = file.FileName,
                    Memory = fileMemoryStream,
                    MimeType = file.MimeType
                };
            }
        }
    }
}