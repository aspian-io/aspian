using System;
using Aspian.Domain.BaseModel;

namespace Aspian.Domain.TaxonomyModel
{
    public interface ITermmeta : IEntitymeta
    {
        string Key { get; set; }
        string Value { get; set; }


        #region Navigation Properties
            Guid TermId { get; set; }
            Term Term { get; set; }
        #endregion
        
    }
}