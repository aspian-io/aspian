using System;

namespace Aspian.Application.Core.TaxonomyServices.DTOs
{
    public class TermPostDto
    {
        public Guid PostId { get; set; }
        public virtual PostDto Post { get; set; }
        public Guid TermTaxonomyId { get; set; }
    }
}