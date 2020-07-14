using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Aspian.Application.Core.TaxonomyServices.DTOs
{
    public class TaxonomyDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string CreatedById { get; set; }
        public virtual UserDto CreatedBy { get; set; }
        public string ModifiedById { get; set; }
        public virtual UserDto ModifiedBy { get; set; }
        public string UserAgent { get; set; }
        public string UserIPAddress { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public virtual TermDto Term { get; set; }
        public Guid? ParentId { get; set; }
        public virtual TaxonomyDto Parent { get; set; }
        public virtual ICollection<TaxonomyDto> ChildTaxonomies { get; set; }
        [JsonPropertyName("posts")]
        public virtual ICollection<TaxonomyPostDto> TaxonomyPosts { get; set; }
    }
}