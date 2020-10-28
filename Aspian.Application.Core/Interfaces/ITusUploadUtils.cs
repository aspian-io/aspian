using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Domain.SiteModel;
using tusdotnet.Interfaces;

namespace Aspian.Application.Core.Interfaces
{
    public interface ITusUploadUtils
    {
        /// <summary>
        /// Transfer uploaded file by Tus from temp folder to destination folder and save its information in database.
        /// </summary>
        /// <param name="file" >Tus File.</param>
        /// <param name="siteType" >Site type.</param>
        /// <param name="refreshToken" >Refresh token.</param>
        /// <param name="refreshToken" >Tus context cancellation token.</param>
        Task TusAddFileAsync(ITusFile file, SiteTypeEnum siteType, string refreshToken, CancellationToken cancellationToken);

        /// <summary>
        /// Delete uploaded file and its chunks by Tus from temp folder.
        /// </summary>
        /// <param name="tusFileId" >File Id that uploaded by Tus.</param>
        /// <param name="absoluteTusTempPath" >Absolute temp folder path.</param>
        Task TusDeleteTempFileAsync(string tusFileId, string absoluteTusTempPath);
    }
}