using System;
using Aspian.Domain.BaseModel;
using Aspian.Domain.PostModel;
using Aspian.Domain.UserModel;

namespace Aspian.Domain.TaxonomyModel
{
    public class TaxonomyPost : EntityInfo, ITaxonomyPost
    {
        public DateTime CreatedAt { get; set; }
        public string CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedById { get; set; }
        public virtual User ModifiedBy { get; set; }
        

        #region Navigation Properties
            public Guid PostId { get; set; }
            public virtual Post Post { get; set; }
            public Guid TaxonomyId { get; set; }
            public virtual Taxonomy Taxonomy { get; set; }
        #endregion
    }
}