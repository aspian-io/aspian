using Aspian.Application.Core.UserServices.DTOs;
using Aspian.Domain.OptionModel;
using Aspian.Domain.SiteModel;
using Aspian.Domain.UserModel;
using AutoMapper;

namespace Aspian.Application.Core.OptionServices.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Option, OptionDto>();
            CreateMap<Site, SiteDto>();
            CreateMap<Optionmeta, OptionmetaDto>();
            CreateMap<User, UserDto>();
        }
    }
}