using System;

namespace Aspian.Application.Core.PostServices.DTOs
{
    public class PostmetaDto
    {
        public Guid Id { get; set; }
        public string MetaKey { get; set; }
        public string MetaValue { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedById { get; set; }
    }
}