using System;
using Aspian.Domain.BaseModel;
using Aspian.Domain.PostModel;

namespace Aspian.Domain.TaxonomyModel
{
    public interface ITaxonomyPost : IEntityCreate, IEntityModify, IEntityInfo
    {
        #region Navigation Properties
            Guid PostId { get; set; }
            Post Post { get; set; }
            Guid TaxonomyId { get; set; }
            Taxonomy Taxonomy { get; set; }
        #endregion
    }
}