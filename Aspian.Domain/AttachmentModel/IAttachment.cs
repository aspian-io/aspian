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
        string MimeType { get; set; }
        string Url { get; set; }
        bool IsMain { get; set; }

        #region Navigation Properties
        ICollection<Attachmentmeta> Attachmentmetas { get; set; }

        Guid? SiteId { get; set; }
        Site Site { get; set; }

        Guid? PhotoOwnerPostId { get; set; }
        Post PhotoOwnerPost { get; set; }
        Guid? VideoOwnerPostId { get; set; }
        Post VideoOwnerPost { get; set; }
        Guid? AudioOwnerPostId { get; set; }
        Post AudioOwnerPost { get; set; }
        Guid? PdfOwnerPostId { get; set; }
        Post PdfOwnerPost { get; set; }
        Guid? TextFileOwnerPostId { get; set; }
        Post TextFileOwnerPost { get; set; }
        Guid? OtherFileOwnerPostId { get; set; }
        Post OtherFileOwnerPost { get; set; }
        Guid? AttachmentOwnerPostId { get; set; }
        Post AttachmentOwnerPost { get; set; }

        string UserId { get; set; }
        User User { get; set; }
        #endregion
    }

    public enum AttachmentTypeEnum
    {
        Photo,
        Video,
        Audio,
        PDF,
        TextFile,
        Other
    }
}