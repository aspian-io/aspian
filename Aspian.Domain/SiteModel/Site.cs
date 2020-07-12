using System;
using System.Collections.Generic;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.AttachmentModel;
using Aspian.Domain.CommentModel;
using Aspian.Domain.OptionModel;
using Aspian.Domain.PostModel;
using Aspian.Domain.TaxonomyModel;

namespace Aspian.Domain.SiteModel
{
    public class Site : ISite
    {
        public Guid Id { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
        public DateTime Registered { get; set; }
        public DateTime? LastUpdated { get; set; }
        public SiteTypeEnum SiteType { get; set; }
        public bool Activated { get; set; }


        #region Navigation Properties
        public virtual ICollection<Option> Options { get; set; }
        public virtual ICollection<ActivityOccurrence> Occurrences { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
        public virtual ICollection<Taxonomy> Taxonomies { get; set; }
        #endregion

    }


}