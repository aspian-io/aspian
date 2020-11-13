using System.IO;
using System.Threading.Tasks;
using Aspian.Application.Core.AttachmentServices.AdminServices;
using Aspian.Domain.AttachmentModel;
using Aspian.Domain.SiteModel;
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
        /// <param name="fileRelativePath" >Relative path of the file.</param>
        Task<string> DeleteFileAsync(string fileRelativePath, UploadLocationEnum uploadLocation);

        /// <summary>
        /// Get requested image stream using <paramref name="imageRelativePath"/> and <paramref name="uploadLocation"/>.
        /// </summary>
        /// <returns>
        /// Stream of the requested image.
        /// </returns>
        /// <param name="imageRelativePath" >Relative path of the image.</param>
        /// <param name="uploadLocation" >The location of a requested image which could be LocalHost or FtpServer.</param>
        Task<MemoryStream> GetImageAsync(string imageRelativePath, UploadLocationEnum uploadLocation);

        /// <summary>
        /// Download requested file using <paramref name="fileRelativePath"/> and <paramref name="uploadLocation"/>.
        /// </summary>
        /// <returns>
        /// Stream of the requested file.
        /// </returns>
        /// <param name="fileRelativePath" >Relative path of the file.</param>
        /// <param name="uploadLocation" >The location of a requested file which could be LocalHost or FtpServer.</param>
        Task<Stream> DownloadFileAsync(string fileRelativePath, long fileSize, UploadLocationEnum uploadLocation);

    }
}