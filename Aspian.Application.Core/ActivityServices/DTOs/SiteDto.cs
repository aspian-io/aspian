using System;
using Aspian.Domain.SiteModel;

namespace Aspian.Application.Core.ActivityServices.DTOs
{
    public class SiteDto
    {
        public Guid Id { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
        public DateTime Registered { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string SiteType { get; set; }
        public bool Activated { get; set; }
    }
}