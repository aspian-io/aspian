using System;
using System.Collections.Generic;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.AttachmentModel;
using Aspian.Domain.BaseModel;
using Aspian.Domain.CommentModel;
using Aspian.Domain.OptionModel;
using Aspian.Domain.PostModel;
using Aspian.Domain.TaxonomyModel;

namespace Aspian.Domain.SiteModel
{
    public interface ISite : IEntityBase, IEntityModify, IEntityInfo
    {
        string Domain { get; set; }
        string Path { get; set; }
        SiteTypeEnum SiteType { get; set; }
        bool IsActivated { get; set; }


        #region Navigation Properties
        ICollection<Option> Options { get; set; }
        ICollection<Activity> Activities { get; set; }
        ICollection<Comment> Comments { get; set; }
        ICollection<Post> Posts { get; set; }
        ICollection<Attachment> Attachments { get; set; }
        ICollection<Taxonomy> Taxonomies { get; set; }
        #endregion
    }

    public enum SiteTypeEnum
    {
        Blog = 0,
        Store = 1,
        LMS = 2,
        eHealth = 3
    }
}