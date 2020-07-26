using System;
using System.Collections.Generic;
using Aspian.Domain.TaxonomyModel;

namespace Aspian.Application.Core.PostServices.AdminServices.DTOs
{
    public class TermDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }

        public DateTime CreatedAt { get; set; }
        public string CreatedById { get; set; }


        #region Navigation Properties
        public virtual ICollection<TermmetaDto> Termmetas { get; set; }
        #endregion
    }
}