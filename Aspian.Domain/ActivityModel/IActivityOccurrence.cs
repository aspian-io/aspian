using System;
using System.Collections.Generic;
using Aspian.Domain.BaseModel;
using Aspian.Domain.SiteModel;
using Aspian.Domain.UserModel;

namespace Aspian.Domain.ActivityModel
{
    public interface IActivityOccurrence : IEntityBase, IEntityCreate
    {
        bool IsRead { get; set; }
        

        #region Navigation Properties
            Guid SiteId { get; set; }
            Site Site { get; set; }
            int ActivityId { get; set; }
            Activity Activity { get; set; }
            ICollection<ActivityOccurrencemeta> Occurrencemetas { get; set; }
        
        #endregion
    }
}