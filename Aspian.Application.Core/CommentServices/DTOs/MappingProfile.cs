using Aspian.Domain.CommentModel;
using Aspian.Domain.UserModel;
using AutoMapper;

namespace Aspian.Application.Core.CommentServices.DTOs
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