using System;
using System.Collections.Generic;
using Aspian.Domain.TaxonomyModel;

namespace Aspian.Application.Core.PostServices.AdminServices.DTOs
{
    public class TaxonomyDto
    {
        public Guid Id { get; set; }
        public TaxonomyTypeEnum Type { get; set; }


        #region Navigaiton Properties
        public virtual TermDto Term { get; set; }
        #endregion
    }
}