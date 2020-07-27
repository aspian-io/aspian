using Aspian.Domain.SiteModel;
using Aspian.Domain.UserModel;
using AutoMapper;

namespace Aspian.Application.Core.SiteServices.AdminServices.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Site, SiteDto>();
            CreateMap<User, UserDto>();

            CreateMap<Edit.Command, Site>()
                .ForMember(d => d.Id, o => o.UseDestinationValue())
                .ForMember(d => d.CreatedAt, o => o.UseDestinationValue());

            CreateMap<DeveloperEdit.Command, Site>()
                .ForMember(d => d.Id, o => o.UseDestinationValue())
                .ForMember(d => d.CreatedAt, o => o.UseDestinationValue());
        }
    }
}