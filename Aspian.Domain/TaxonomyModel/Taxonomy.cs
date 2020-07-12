using System;
using System.Collections.Generic;
using Aspian.Domain.BaseModel;
using Aspian.Domain.SiteModel;

namespace Aspian.Domain.TaxonomyModel
{
    public class Taxonomy : Entitymeta, ITaxonomy
    {
        public TaxonomyEnum Name { get; set; }
        public string Description { get; set; }

        
        #region Navigaiton Properties
            public Guid? ParentId { get; set; }
            public virtual Taxonomy Parent { get; set; }
            public virtual ICollection<Taxonomy> ChildTaxonomies { get; set; }
            public virtual ICollection<TaxonomyPost> TaxonomyPosts { get; set; }
            public virtual Term Term { get; set; }
            public Guid SiteId { get; set; }
            public virtual Site Site { get; set; }
        #endregion
    }
}