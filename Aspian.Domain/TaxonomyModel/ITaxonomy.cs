using System;
using System.Collections.Generic;
using Aspian.Domain.BaseModel;
using Aspian.Domain.SiteModel;

namespace Aspian.Domain.TaxonomyModel
{
    public interface ITaxonomy : IEntitymeta
    {
        TaxonomyEnum Name { get; set; }
        string Description { get; set; }


        #region Navigaiton Properties
            Guid? ParentId { get; set; }
            Taxonomy Parent { get; set; }
            ICollection<Taxonomy> ChildTaxonomies { get; set; }
            ICollection<TaxonomyPost> TaxonomyPosts { get; set; }
            Term Term { get; set; }
            Guid SiteId { get; set; }
            Site Site { get; set; }
        #endregion
    }

    public enum TaxonomyEnum {
        nav_menu,
        category,
        tag
    }
}