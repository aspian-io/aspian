using System;

namespace Aspian.Application.Core.PostServices.AdminServices.DTOs
{
    public class TermmetaDto
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedById { get; set; }

    }
}