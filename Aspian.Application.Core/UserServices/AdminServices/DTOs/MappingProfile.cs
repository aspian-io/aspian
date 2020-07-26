using System.Linq;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.AttachmentModel;
using Aspian.Domain.CommentModel;
using Aspian.Domain.PostModel;
using Aspian.Domain.SiteModel;
using Aspian.Domain.TaxonomyModel;
using Aspian.Domain.UserModel;
using AutoMapper;

namespace Aspian.Application.Core.UserServices.AdminServices.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, CurrentUserDto>();
            CreateMap<Attachment, PhotoDto>();

            CreateMap<User, UserDto>()
                .ForMember(d => d.Taxonomies, o => o.MapFrom(s => s.CreatedTaxonomies.Count))
                .ForMember(d => d.Posts, o => o.MapFrom(s => s.CreatedPosts.Count))
                .ForMember(d => d.Attachments, o => o.MapFrom(s => s.CreatedAttachments.Count))
                .ForMember(d => d.Activites, o => o.MapFrom(s => s.Activities.Count))
                .ForMember(d => d.Photo, o => o.MapFrom(s => s.CreatedAttachments.SingleOrDefault(x => x.Type == AttachmentTypeEnum.Photo && x.IsMain)));

        }
    }
}