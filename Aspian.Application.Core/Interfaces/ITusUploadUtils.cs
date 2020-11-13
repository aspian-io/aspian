using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Domain.AttachmentModel;
using Aspian.Domain.SiteModel;
using tusdotnet.Interfaces;

namespace Aspian.Application.Core.Interfaces
{
    public interface ITusUploadUtils
    {
        /// <summary>
        /// Gets dynamic upload store path based on username.
        /// </summary>
        /// <param name="uploadForlderName" >Upload's root folder name.</param>
        /// <param name="uploadLocation" >Either LocalHost or FtpServer.</param>
        /// <param name="linkAccessibility" >Upload's link being Private or Public.</param>
        string GetStorePath(string uploadForlderName, 
            UploadLocationEnum uploadLocation = UploadLocationEnum.LocalHost,
            UploadLinkAccessibilityEnum linkAccessibility = UploadLinkAccessibilityEnum.Private);
        /// <summary>
        /// Save uploaded Tus file info into database after upload is complete.
        /// </summary>
        /// <param name="file" >Tus File.</param>
        /// <param name="siteType" >Site type.</param>
        /// <param name="refreshToken" >Refresh token.</param>
        /// <param name="location" >LocalHost or FTPServer.</param>
        /// <param name="cancellationToken" >Cancelation Token.</param>
        Task SaveTusFileInfoAsync(ITusFile file, SiteTypeEnum siteType, string refreshToken, UploadLocationEnum location, CancellationToken cancellationToken);

        /// <summary>
        /// Delete uploaded file and its chunks by Tus from database and storage.
        /// </summary>
        /// <param name="tusFileId" >File Id that uploaded by Tus or Tus file name.</param>
        /// <param name="fileRelativePath" >File relative path.</param>
        Task DeleteTusFileAsync(string tusFileId, string fileRelativePath);
    }
}