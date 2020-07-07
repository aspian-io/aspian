using Aspian.Domain.PostModel;
using AutoMapper;

namespace Aspian.Application.Core.PostServices.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Create.Command, Post>();
        }
    }
}