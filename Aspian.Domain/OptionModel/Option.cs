using System;
using System.Collections.Generic;
using Aspian.Domain.BaseModel;
using Aspian.Domain.SiteModel;

namespace Aspian.Domain.OptionModel
{
    public class Option : EntityBase, IOption
    {
        public SectionEnum Section { get; set; }
        public string Description { get; set; }
        

        #region Navigation Properties
        public Guid SiteId { get; set; }
        public virtual Site Site { get; set; }
        public virtual ICollection<Optionmeta> Optionmetas { get; set; }
        #endregion
    }

    
}