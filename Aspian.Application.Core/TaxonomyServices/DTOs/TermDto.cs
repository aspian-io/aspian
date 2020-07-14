using System;

namespace Aspian.Application.Core.TaxonomyServices.DTOs
{
    public class TermDto
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
        public string Name { get; set; }
        public string Slug { get; set; }
    }
}