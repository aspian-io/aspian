using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.AttachmentServices.DTOs;
using Aspian.Application.Core.Errors;
using Aspian.Application.Core.Interfaces;
using Aspian.Domain.AttachmentModel;
using Aspian.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.AttachmentServices
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
            public Handler(DataContext context, IUploadAccessor uploadAccessor)
            {
                _uploadAccessor = uploadAccessor;
                _context = context;
            }

            public async Task<GetImageDto> Handle(Query request, CancellationToken cancellationToken)
            {
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