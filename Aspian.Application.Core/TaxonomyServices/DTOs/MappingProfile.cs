using Aspian.Domain.PostModel;
using Aspian.Domain.TaxonomyModel;
using AutoMapper;

namespace Aspian.Application.Core.TaxonomyServices.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Taxonomy, TaxonomyDto>();
            CreateMap<Term, TermDto>();
            CreateMap<Post, PostDto>();
            CreateMap<TaxonomyPost, TaxonomyPostDto>();
            CreateMap<Create.Command, Taxonomy>()
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForPath(d => d.Term.Id, o => o.Ignore())
                .ForPath(d => d.Term.Name, o => o.MapFrom(s => s.Term.Name))
                .ForPath(d => d.Term.Slug, o => o.MapFrom(s => s.Term.Slug));
            CreateMap<Edit.Command, Taxonomy>()
                .ForMember(d => d.CreatedAt, o => o.Ignore())
                .ForPath(d => d.Term.Id, o => o.Ignore())
                .ForPath(d => d.Term.Name, o => o.MapFrom(s => s.Term.Name))
                .ForPath(d => d.Term.Slug, o => o.MapFrom(s => s.Term.Slug));
        }
    }
}