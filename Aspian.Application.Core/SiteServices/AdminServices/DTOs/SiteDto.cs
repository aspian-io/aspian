using System;
using Aspian.Domain.SiteModel;

namespace Aspian.Application.Core.SiteServices.AdminServices.DTOs
{
    public class SiteDto
    {
        public Guid Id { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
        public string SiteType { get; set; }
        public bool IsActivated { get; set; }
        public DateTime? ModuleActivatedAt { get; set; }
        public DateTime? ModuleExpiresAt { get; set; }
        public DateTime? MainHostActivatedAt { get; set; }
        public DateTime? MainHostExpiresAt { get; set; }
        public long MainHostCapacity { get; set; }
        public long MainHostAvailableSpace { get; set; }
        public bool HasDownloadHost { get; set; }
        public DateTime? DownloadHostActivatedAt { get; set; }
        public DateTime? DownloadHostExpiresAt { get; set; }
        public long? DownloadHostCapacity { get; set; }
        public long? DownloadHostAvailableSpace { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public virtual UserDto ModifiedBy { get; set; }
    }
}