using System;

namespace Aspian.Application.Core.TaxonomyService
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string  Subtitle { get; set; }
        public string Excerpt { get; set; }
        public string Content { get; set; }
        public string Slug { get; set; }
    }
}