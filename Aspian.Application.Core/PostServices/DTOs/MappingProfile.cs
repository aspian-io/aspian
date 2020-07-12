using System.Linq;
using Aspian.Domain.AttachmentModel;
using Aspian.Domain.PostModel;
using Aspian.Domain.TaxonomyModel;
using Aspian.Domain.UserModel;
using AutoMapper;

namespace Aspian.Application.Core.PostServices.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Create.Command, Post>();
            CreateMap<Post, PostDto>(); ;
            CreateMap<Postmeta, PostmetaDto>();
            CreateMap<User, UserDto>();
            CreateMap<Attachment, AttachmentDto>();
            CreateMap<Attachmentmeta, AttachmentmetaDto>();
            CreateMap<TaxonomyPost, TaxonomyPostDto>();
            CreateMap<Taxonomy, TaxonomyDto>();
            CreateMap<Term, TermDto>();
            CreateMap<Termmeta, TermmetaDto>();

        }
    }
}