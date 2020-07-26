using Aspian.Domain.PostModel;
using Aspian.Domain.TaxonomyModel;
using Aspian.Domain.UserModel;
using AutoMapper;

namespace Aspian.Application.Core.TaxonomyServices.AdminServices.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Taxonomy, TaxonomyDto>();
            CreateMap<Term, TermDto>();
            CreateMap<TermDto, Term>()
                .ForMember(d => d.CreatedAt, o => o.UseDestinationValue())
                .ForMember(d => d.CreatedById, o => o.UseDestinationValue());
            CreateMap<Post, PostDto>();
            CreateMap<TaxonomyPost, TaxonomyPostDto>();
            CreateMap<Create.Command, Taxonomy>()
                .ForPath(d => d.Term.Id, o => o.Ignore())
                .ForPath(d => d.Term.Name, o => o.MapFrom(s => s.Term.Name))
                .ForPath(d => d.Term.Slug, o => o.MapFrom(s => s.Term.Slug));
            CreateMap<Edit.Command, Taxonomy>()
                .ForMember(d => d.CreatedAt, o => o.UseDestinationValue())
                .ForMember(d => d.CreatedById, o => o.UseDestinationValue())
                .ForPath(d => d.Term.Id, o => o.Ignore())
                .ForPath(d => d.Term.Name, o => o.MapFrom(s => s.Term.Name))
                .ForPath(d => d.Term.Slug, o => o.MapFrom(s => s.Term.Slug));
            CreateMap<User, UserDto>();
        }
    }
}