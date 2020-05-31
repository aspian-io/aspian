using System;
using Aspian.Domain.BaseModel;

namespace Aspian.Domain.TaxonomyModel
{
    public class Termmeta : Entitymeta, ITermmeta
    {
        public string Key { get; set; }
        public string Value { get; set; }


        #region Navigation Properties
            public Guid TermId { get; set; }
            public virtual Term Term { get; set; }
        #endregion
        
    }
}