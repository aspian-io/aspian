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
            Guid TaxonomyId { get; set; }
            Taxonomy Taxonomy { get; set; }
            ICollection<Termmeta> Termmetas { get; set; }
        #endregion
    }
}