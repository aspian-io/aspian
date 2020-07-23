using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Errors;
using Aspian.Application.Core.Interfaces;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.AttachmentModel;
using Aspian.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.AttachmentServices.AdminServices
{
    public class Delete
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IUploadAccessor _uploadAccessor;
            private readonly IActivityLogger _logger;
            public Handler(DataContext context, IUploadAccessor uploadAccessor, IActivityLogger logger)
            {
                _logger = logger;
                _uploadAccessor = uploadAccessor;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var attachment = await _context.Attachments.SingleOrDefaultAsync(x => x.Id == request.Id);

                if (attachment == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Attachment = "Not found" });

                if (attachment.UploadLocation == UploadLocationEnum.LocalHost)
                {
                    var result = await _uploadAccessor.DeleteFileAsync(attachment.RelativePath, UploadLocationEnum.LocalHost);
                    if (result != "ok")
                        throw new Exception("Problem deleting the file!");
                }

                if (attachment.UploadLocation == UploadLocationEnum.FtpServer)
                {
                    var result = await _uploadAccessor.DeleteFileAsync(attachment.RelativePath, UploadLocationEnum.FtpServer);
                    if (result != "ok")
                        throw new Exception("Problem deleting the file!");
                }

                _context.Attachments.Remove(attachment);

                var success = await _context.SaveChangesAsync() > 0;

                if (success)
                {
                    await _logger.LogActivity(
                        attachment.SiteId, 
                        ActivityCodeEnum.AttachmentDelete,
                        ActivitySeverityEnum.Low,
                        ActivityObjectEnum.Attachemnt,
                        $"The {attachment.Type} with the name {attachment.FileName} deleted!");

                    return Unit.Value;
                }

                throw new Exception("Problem saving changes!");
            }
        }
    }
}