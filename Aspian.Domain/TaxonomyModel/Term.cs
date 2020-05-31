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
            public Guid TermTaxonomyId { get; set; }
            public virtual TermTaxonomy TermTaxonomy { get; set; }
            public virtual ICollection<Termmeta> Termmetas { get; set; }
        #endregion
    }
}