using System;
using Aspian.Domain.SiteModel;

namespace Aspian.Application.Core.SiteServices.AdminServices.DTOs
{
    public class SiteDto
    {
        public Guid Id { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
        public DateTime Registered { get; set; }
        public DateTime? LastUpdated { get; set; }
        public SiteTypeEnum SiteType { get; set; }
        public bool Activated { get; set; }
    }
}