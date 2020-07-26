using Aspian.Domain.SiteModel;
using AutoMapper;

namespace Aspian.Application.Core.SiteServices.AdminServices.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Site, SiteDto>();
        }
    }
}