using System;
using Aspian.Domain.BaseModel;
using Aspian.Domain.PostModel;
using Aspian.Domain.UserModel;

namespace Aspian.Domain.TaxonomyModel
{
    public class TermPost : EntityInfo, ITermPost
    {
        public DateTime CreatedDate { get; set; }
        public string CreatedById { get; set; }
        public User CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedById { get; set; }
        public User ModifiedBy { get; set; }
        public int Version { get; set; }
        

        #region Navigation Properties
            public Guid PostId { get; set; }
            public virtual Post Post { get; set; }
            public Guid TermTaxonomyId { get; set; }
            public virtual TermTaxonomy TermTaxonomy { get; set; }
        #endregion
    }
}