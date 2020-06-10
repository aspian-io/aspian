using System.Linq;
using Aspian.Domain.AttachmentModel;
using Aspian.Domain.UserModel;
using AutoMapper;

namespace Aspian.Application.Core.UserServices.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserProfileDto>()
                .ForPath(d => d.Image, o => o.MapFrom(s => s.CreatedAttachments.SingleOrDefault(x => x.Type == AttachmentTypeEnum.Photo && x.IsMain).Url))
                .ForPath(d => d.Photos, o => o.MapFrom(s => s.CreatedAttachments.Where(x => x.Type == AttachmentTypeEnum.Photo)));
            CreateMap<Attachment, PhotoDto>();
        }
    }
}