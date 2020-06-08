using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Interfaces;
using Aspian.Application.Core.Photos.DTOs;
using Aspian.Domain.AttachmentModel;
using Aspian.Domain.PostModel;
using Aspian.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.Photos
{
    public class Add
    {
        public class Command : IRequest<PhotoDto>
        {
            public IFormFile File { get; set; }
        }

        public class Handler : IRequestHandler<Command, PhotoDto>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly IPhotoAccessor _photoAccessor;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IUserAccessor userAccessor, IPhotoAccessor photoAccessor, IMapper mapper)
            {
                _mapper = mapper;
                _photoAccessor = photoAccessor;
                _userAccessor = userAccessor;
                _context = context;
            }

            public async Task<PhotoDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var photoUploadResult = _photoAccessor.AddPhoto(request.File);

                var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUsername());

                var photo = new PhotoDto
                {
                    Url = photoUploadResult.Url,
                };

                if (!user.Photos.Any(x => x.IsMain))
                    photo.IsMain = true;

                var userPhotos = _mapper.Map<PhotoDto, Attachment>(photo);

                user.Photos.Add(userPhotos);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return photo;

                throw new Exception("Problem saving changes!");
            }
        }
    }
}