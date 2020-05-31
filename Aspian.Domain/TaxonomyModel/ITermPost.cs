using System;
using Aspian.Domain.BaseModel;
using Aspian.Domain.PostModel;

namespace Aspian.Domain.TaxonomyModel
{
    public interface ITermPost : IEntityCreate, IEntityModify, IEntityInfo
    {
        #region Navigation Properties
            Guid PostId { get; set; }
            Post Post { get; set; }
            Guid TermTaxonomyId { get; set; }
            TermTaxonomy TermTaxonomy { get; set; }
        #endregion
    }
}