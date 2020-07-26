using System;

namespace Aspian.Application.Core.TaxonomyServices.AdminServices.DTOs
{
    public class TaxonomyPostDto
    {
        public Guid PostId { get; set; }
        public virtual PostDto Post { get; set; }
        public Guid TaxonomyId { get; set; }
    }
}