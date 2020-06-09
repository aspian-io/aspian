using System.Threading.Tasks;
using Aspian.Application.Core.AttachmentServices;
using Aspian.Domain.AttachmentModel;
using Microsoft.AspNetCore.Http;

namespace Aspian.Application.Core.Interfaces
{
    public interface IFileAccessor
    {
        /// <summary>
        /// Upload IFormFile <paramref name="file"/> and returns the result.
        /// </summary>
        /// <returns>
        /// FileUploadResultDto class which will be containing uploaded file.
        /// </returns>
        /// <param name="file" >The file you want to upload which must be an IFormFile type.</param>
        Task<FileUploadResult> AddFileAsync(IFormFile file);

        /// <summary>
        /// To determine AttachmentType by getting <paramref name="file"/>.
        /// </summary>
        /// <returns>
        /// Attachment type.
        /// </returns>
        /// <param name="file" >The file you want to upload which must be an IFormFile type.</param>
        AttachmentTypeEnum CheckFileType(IFormFile file);
    }
}