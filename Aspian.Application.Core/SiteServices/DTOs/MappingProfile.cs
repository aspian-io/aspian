using Aspian.Domain.SiteModel;
using AutoMapper;

namespace Aspian.Application.Core.SiteServices.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Site, SiteDto>();
        }
    }
}