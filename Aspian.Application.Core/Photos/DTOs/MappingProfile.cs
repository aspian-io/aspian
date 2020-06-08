using Aspian.Domain.AttachmentModel;
using AutoMapper;

namespace Aspian.Application.Core.Photos.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PhotoDto, Attachment>();
        }
    }
}