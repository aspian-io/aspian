using Aspian.Application.Core.CommentServices.AdminServices;
using Aspian.Domain.CommentModel;
using Aspian.Domain.UserModel;
using AutoMapper;

namespace Aspian.Application.Core.CommentServices.AdminServices.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Create.Command, Comment>();
            CreateMap<Comment, CommentDto>();
            CreateMap<User, UserDto>();
        }
    }
}