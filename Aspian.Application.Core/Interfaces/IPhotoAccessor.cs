using Aspian.Application.Core.Photos;
using Microsoft.AspNetCore.Http;

namespace Aspian.Application.Core.Interfaces
{
    public interface IPhotoAccessor
    {
        PhotoUploadResult AddPhoto(IFormFile file);
        string DeletePhoto(string publicId);
    }
}