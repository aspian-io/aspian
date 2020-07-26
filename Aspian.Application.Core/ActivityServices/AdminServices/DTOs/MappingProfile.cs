using Aspian.Domain.ActivityModel;
using Aspian.Domain.SiteModel;
using Aspian.Domain.UserModel;
using AutoMapper;

namespace Aspian.Application.Core.ActivityServices.AdminServices.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Activity, ActivityDto>();
            CreateMap<Site, SiteDto>();
            CreateMap<User, UserDto>();
        }
    }
}