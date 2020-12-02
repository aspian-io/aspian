using System;
using System.Collections.Generic;
using Aspian.Domain.BaseModel;
using Aspian.Domain.SiteModel;

namespace Aspian.Domain.OptionModel
{
    public interface IOption : IEntityBase
    {
        SectionEnum Section { get; set; }
        string Description { get; set; }


        #region Navigation Properties
        Guid SiteId { get; set; }
        Site Site { get; set; }
        ICollection<Optionmeta> Optionmetas { get; set; }
        #endregion
    }

    public enum SectionEnum
    {
        AdminActivity,
        Comment,
        AdminPost,
        AdminTaxonomy,
        User,
        AdminAttachment,
        AdminAttachmentFileTypes
    }
}