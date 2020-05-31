using System;
using System.Collections.Generic;
using Aspian.Domain.BaseModel;

namespace Aspian.Domain.TaxonomyModel
{
    public interface ITerm : IEntitymeta
    {
        string Name { get; set; }
        string Slug { get; set; }
        
        
        #region Navigation Properties
            Guid TermTaxonomyId { get; set; }
            TermTaxonomy TermTaxonomy { get; set; }
            ICollection<Termmeta> Termmetas { get; set; }
        #endregion
    }
}