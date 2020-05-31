using System;
using Aspian.Domain.BaseModel;

namespace Aspian.Domain.ActivityModel
{
    public interface IActivityOccurrencemeta : IEntityBase, IEntityCreate
    {
        string Name { get; set; }
        string Value { get; set; }
        

        #region Navigation Properties
            Guid OccurenceId { get; set; }
            ActivityOccurrence Occurrence { get; set; }
        #endregion
    }
}