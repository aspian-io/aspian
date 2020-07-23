using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Errors;
using Aspian.Application.Core.Interfaces;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.AttachmentModel;
using Aspian.Domain.SiteModel;
using Aspian.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.AttachmentServices.UserServices
{
    public class SetMainPhoto
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly IActivityLogger _logger;
            public Handler(DataContext context, IUserAccessor userAccessor, IActivityLogger logger)
            {
                _logger = logger;
                _userAccessor = userAccessor;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var site = await _context.Sites.FirstOrDefaultAsync(x => x.SiteType == SiteTypeEnum.Blog);
                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUsername());

                var photo = user.CreatedAttachments.FirstOrDefault(x => x.Id == request.Id && x.Type == AttachmentTypeEnum.Photo);

                if (photo == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Photo = "Not found" });

                var currentMain = user.CreatedAttachments.FirstOrDefault(x => x.Type == AttachmentTypeEnum.Photo && x.IsMain);

                currentMain.IsMain = false;
                photo.IsMain = true;

                var success = await _context.SaveChangesAsync() > 0;

                if (success) 
                {
                    await _logger.LogActivity(
                        site.Id, 
                        ActivityCodeEnum.AttachmentSetMainPhoto, 
                        ActivitySeverityEnum.Information, 
                        ActivityObjectEnum.Attachemnt, 
                        $"The main photo changed");

                    return Unit.Value;
                }

                throw new Exception("Problem saving changes!");
            }
        }
    }
}