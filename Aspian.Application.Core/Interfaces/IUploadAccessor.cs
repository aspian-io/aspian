using System.Threading.Tasks;
using Aspian.Application.Core.AttachmentServices;
using Aspian.Domain.AttachmentModel;
using Microsoft.AspNetCore.Http;

namespace Aspian.Application.Core.Interfaces
{
    public interface IUploadAccessor
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
        /// Delete file by using <paramref name="filePath"/>.
        /// </summary>
        /// <returns>
        /// a string containing "ok" if the deletion process is successful.
        /// </returns>
        /// <exception cref="System.IO.IOException">IF there is a problem with filePath or the file while deleting.</exception>
        /// <param name="filePath" >Absolute path of the file.</param>
        public string DeleteFile(string filePath);

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