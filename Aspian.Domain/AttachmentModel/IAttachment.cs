using System;
using System.Collections.Generic;
using Aspian.Domain.BaseModel;
using Aspian.Domain.PostModel;
using Aspian.Domain.SiteModel;
using Aspian.Domain.UserModel;

namespace Aspian.Domain.AttachmentModel
{
    public interface IAttachment : IEntitymeta
    {
        AttachmentTypeEnum Type { get; set; }
        string FileName { get; set; }
        string FileExtension { get; set; }
        string FileSize { get; set; }
        string MimeType { get; set; }
        UploadLocationEnum UploadLocation { get; set; }
        string RelativePath { get; set; }
        bool IsMain { get; set; }

        #region Navigation Properties
        ICollection<PostAttachment> PostAttachments { get; set; }
        ICollection<Attachmentmeta> Attachmentmetas { get; set; }

        Guid SiteId { get; set; }
        Site Site { get; set; }
        #endregion
    }

    public enum AttachmentTypeEnum
    {
        Photo,
        Video,
        Audio,
        PDF,
        TextFile,
        Compressed,
        Other
    }

    public enum UploadLocationEnum
    {
        LocalHost,
        FtpServer
    }
}