using System.Linq;
using Aspian.Application.Core.PostServices.AdminServices;
using Aspian.Domain.AttachmentModel;
using Aspian.Domain.PostModel;
using Aspian.Domain.SiteModel;
using Aspian.Domain.TaxonomyModel;
using Aspian.Domain.UserModel;
using AutoMapper;

namespace Aspian.Application.Core.PostServices.AdminServices.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Create.Command, Post>();

            CreateMap<Edit.Command, Post>()
                .ForMember(d => d.Id, o => o.UseDestinationValue())
                .ForMember(d => d.CreatedAt, o => o.UseDestinationValue())
                .ForMember(d => d.CreatedById, o => o.UseDestinationValue());

            CreateMap<PostDto, Post>()
                .ForMember(d => d.Id, o => o.UseDestinationValue())
                .ForMember(d => d.CreatedAt, o => o.UseDestinationValue())
                .ForMember(d => d.CreatedById, o => o.UseDestinationValue());
            CreateMap<PostmetaDto, Postmeta>()
                .ForMember(d => d.Id, o => o.UseDestinationValue())
                .ForMember(d => d.CreatedAt, o => o.UseDestinationValue())
                .ForMember(d => d.CreatedById, o => o.UseDestinationValue());
            CreateMap<UserDto, User>()
                .ForMember(d => d.Id, o => o.UseDestinationValue());
            CreateMap<PostAttachmentDto, PostAttachment>()
                .ForMember(d => d.CreatedAt, o => o.UseDestinationValue())
                .ForMember(d => d.CreatedById, o => o.UseDestinationValue());
            CreateMap<AttachmentDto, Attachment>()
                .ForMember(d => d.Id, o => o.UseDestinationValue())
                .ForMember(d => d.CreatedAt, o => o.UseDestinationValue())
                .ForMember(d => d.CreatedById, o => o.UseDestinationValue());
            CreateMap<AttachmentmetaDto, Attachmentmeta>()
                .ForMember(d => d.Id, o => o.UseDestinationValue())
                .ForMember(d => d.CreatedAt, o => o.UseDestinationValue())
                .ForMember(d => d.CreatedById, o => o.UseDestinationValue());
            CreateMap<TaxonomyPostDto, TaxonomyPost>()
                .ForMember(d => d.CreatedAt, o => o.UseDestinationValue())
                .ForMember(d => d.CreatedById, o => o.UseDestinationValue());
            CreateMap<TaxonomyDto, Taxonomy>()
                .ForMember(d => d.Id, o => o.UseDestinationValue())
                .ForMember(d => d.CreatedAt, o => o.UseDestinationValue())
                .ForMember(d => d.CreatedById, o => o.UseDestinationValue());
            CreateMap<TermDto, Term>()
                .ForMember(d => d.Id, o => o.UseDestinationValue())
                .ForMember(d => d.CreatedAt, o => o.UseDestinationValue())
                .ForMember(d => d.CreatedById, o => o.UseDestinationValue());
            CreateMap<TermmetaDto, Termmeta>()
                .ForMember(d => d.Id, o => o.UseDestinationValue())
                .ForMember(d => d.CreatedAt, o => o.UseDestinationValue())
                .ForMember(d => d.CreatedById, o => o.UseDestinationValue());


            CreateMap<Post, PostDto>()
                .ForMember(d => d.Comments, o => o.MapFrom(s => s.Comments.Count));
            CreateMap<Postmeta, PostmetaDto>();
            CreateMap<Attachment, PhotoDto>();
            CreateMap<User, UserDto>()
                .ForMember(d => d.ProfilePhoto, o => o.MapFrom(s => s.CreatedAttachments.SingleOrDefault(x => x.Type == AttachmentTypeEnum.Photo && x.IsMain)));
            CreateMap<PostAttachment, PostAttachmentDto>();
            CreateMap<Attachment, AttachmentDto>();
            CreateMap<Attachmentmeta, AttachmentmetaDto>();
            CreateMap<TaxonomyPost, TaxonomyPostDto>();
            CreateMap<Taxonomy, TaxonomyDto>();
            CreateMap<Term, TermDto>();
            CreateMap<Termmeta, TermmetaDto>();
            CreateMap<Site, SiteDto>();

            CreateMap<Post, AntParentPostTreeSelectDto>()
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
                .ForMember(d => d.Value, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Key, o => o.MapFrom(s => s.Id));

        }
    }
}