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
        string PublicFileName { get; set; }
        string FileExtension { get; set; }
        long FileSize { get; set; }
        string MimeType { get; set; }
        UploadLinkAccessibilityEnum LinkAccessibility { get; set; }
        UploadLocationEnum UploadLocation { get; set; }
        string RelativePath { get; set; }
        string ThumbnailPath { get; set; }
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
        Photo = 0,
        Video = 1,
        Audio = 2,
        PDF = 3,
        TextFile = 4,
        Compressed = 5,
        Other = 6
    }

    public enum UploadLocationEnum
    {
        LocalHost,
        FtpServer
    }

    public enum UploadLinkAccessibilityEnum
    {
        Public,
        Private
    }
}