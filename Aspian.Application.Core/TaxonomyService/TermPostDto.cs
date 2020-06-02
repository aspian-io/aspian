using System;
using Aspian.Domain.PostModel;
using Aspian.Domain.TaxonomyModel;

namespace Aspian.Application.Core.TaxonomyService
{
    public class TermPostDto
    {
        public Guid PostId { get; set; }
        public virtual PostDto Post { get; set; }
        public Guid TermTaxonomyId { get; set; }
    }
}