using System;
using System.Collections.Generic;
using Aspian.Domain.BaseModel;
using Aspian.Domain.SiteModel;

namespace Aspian.Domain.TaxonomyModel
{
    public class TermTaxonomy : Entitymeta, ITermTaxonomy
    {
        public TaxonomyEnum Taxonomy { get; set; }
        public string Description { get; set; }

        
        #region Navigaiton Properties
            public Guid? ParentId { get; set; }
            public virtual TermTaxonomy Parent { get; set; }
            public virtual ICollection<TermTaxonomy> ChildTaxonomies { get; set; }
            public virtual ICollection<TermPost> TermPosts { get; set; }
            public virtual Term Term { get; set; }
            public Guid SiteId { get; set; }
            public virtual Site Site { get; set; }
        #endregion
    }
}