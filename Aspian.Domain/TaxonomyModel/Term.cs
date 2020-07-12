using System;
using System.Collections.Generic;
using Aspian.Domain.BaseModel;

namespace Aspian.Domain.TaxonomyModel
{
    public class Term : Entitymeta, ITerm
    {
        public string Name { get; set; }
        public string Slug { get; set; }

        
        #region Navigation Properties
            public Guid TaxonomyId { get; set; }
            public virtual Taxonomy Taxonomy { get; set; }
            public virtual ICollection<Termmeta> Termmetas { get; set; }
        #endregion
    }
}