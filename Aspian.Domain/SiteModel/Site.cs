using System;
using System.Collections.Generic;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.AttachmentModel;
using Aspian.Domain.BaseModel;
using Aspian.Domain.CommentModel;
using Aspian.Domain.OptionModel;
using Aspian.Domain.PostModel;
using Aspian.Domain.TaxonomyModel;
using Aspian.Domain.UserModel;

namespace Aspian.Domain.SiteModel
{
    public class Site : EntityInfo, ISite
    {
        public Guid Id { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
        public SiteTypeEnum SiteType { get; set; }
        public bool IsActivated { get; set; }
        public DateTime? ModuleActivatedAt { get; set; }
        public DateTime? ModuleExpiresAt { get; set; }
        public DateTime? MainHostActivatedAt { get; set; }
        public DateTime? MainHostExpiresAt { get; set; }
        public long MainHostCapacity { get; set; }
        public long MainHostAvailableSpace { get; set; }
        public bool HasDownloadHost { get; set; }
        public DateTime? DownloadHostActivatedAt { get; set; }
        public DateTime? DownloadHostExpiresAt { get; set; }
        public long? DownloadHostCapacity { get; set; }
        public long? DownloadHostAvailableSpace { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedById { get; set; }
        public virtual User ModifiedBy { get; set; }


        #region Navigation Properties
        public virtual ICollection<Option> Options { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
        public virtual ICollection<Taxonomy> Taxonomies { get; set; }
        #endregion

    }


}