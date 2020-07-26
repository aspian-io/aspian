using Aspian.Domain.AttachmentModel;
using Aspian.Domain.UserModel;
using AutoMapper;

namespace Aspian.Application.Core.UserServices.UserServices.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<User, UserProfileDto>();
            CreateMap<Attachment, PhotoDto>();
        }
    }
}