using System;
using Aspian.Domain.TaxonomyModel;

namespace Aspian.Application.Core.PostServices.DTOs
{
    public class TaxonomyPostDto
    {
        public Guid TaxonomyId { get; set; }
        public virtual TaxonomyDto Taxonomy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedById { get; set; }
    }
}