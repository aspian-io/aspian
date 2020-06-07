using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Interfaces;
using Aspian.Application.Core.Photos.DTOs;
using Aspian.Domain.PostModel;
using Aspian.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.Photos
{
    public class Add
    {
        public class Command : IRequest<Photo>
        {
            public IFormFile File { get; set; }
        }

        public class Handler : IRequestHandler<Command, Photo>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly IPhotoAccessor _photoAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor, IPhotoAccessor photoAccessor)
            {
                _photoAccessor = photoAccessor;
                _userAccessor = userAccessor;
                _context = context;
            }

            public async Task<Photo> Handle(Command request, CancellationToken cancellationToken)
            {
                var photoUploadResult = _photoAccessor.AddPhoto(request.File);

                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUsername());

                var photo = new Photo
                {
                    Url = photoUploadResult.Url,
                    Id = photoUploadResult.PublicId
                };

                if (!user.Photos.Any(x => x.Postmetas.Any(pm => pm.MetaKey == PhotoUploadFieldsEnum.IsMain.ToString() && pm.MetaValue == "true")))
                    photo.IsMain = true;

                var postPhotoType = new Post
                {
                    PostStatus = PostStatusEnum.Inherit,
                    Type = PostTypeEnum.Photo,
                    MimeType = "",
                    Postmetas = new List<Postmeta> {
                        new Postmeta {
                            MetaKey = PhotoUploadFieldsEnum.Id.ToString(),
                            MetaValue = photo.Id
                        },
                        new Postmeta {
                            MetaKey = PhotoUploadFieldsEnum.Url.ToString(),
                            MetaValue = photo.Url
                        },
                        new Postmeta {
                            MetaKey = PhotoUploadFieldsEnum.IsMain.ToString(),
                            MetaValue = photo.IsMain.ToString()
                        }
                    }
                };

                user.Photos.Add(postPhotoType);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return photo;

                throw new Exception("Problem saving changes!");
            }
        }
    }
}