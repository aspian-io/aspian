using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Aspian.Domain.BaseModel;
using Aspian.Domain.TaxonomyModel;

namespace Aspian.Application.Core.TaxonomyServices.DTOs
{
    public class TaxonomyDto : Entitymeta
    {
        public string Taxonomy { get; set; }
        public string Description { get; set; }
        public virtual TermDto Term { get; set; }
        public Guid? ParentId { get; set; }
        public virtual Taxonomy Parent { get; set; }
        public virtual ICollection<Taxonomy> ChildTaxonomies { get; set; }
        [JsonPropertyName("posts")]
        public virtual ICollection<TaxonomyPostDto> TaxonomyPosts { get; set; }
    }
}