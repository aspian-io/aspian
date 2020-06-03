using Aspian.Domain.PostModel;
using Aspian.Domain.TaxonomyModel;
using AutoMapper;

namespace Aspian.Application.Core.TaxonomyServices.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TermTaxonomy, TaxonomyDto>();
            CreateMap<Term, TermDto>();
            CreateMap<Post, PostDto>();
            CreateMap<TermPost, TermPostDto>();
            CreateMap<Create.Command, TermTaxonomy>()
                .ForMember(d => d.CreatedDate, o => o.Ignore())
                .ForPath(d => d.Term.Id, o => o.Ignore())
                .ForPath(d => d.Term.Name, o => o.MapFrom(s => s.Term.Name))
                .ForPath(d => d.Term.Slug, o => o.MapFrom(s => s.Term.Slug));
            CreateMap<Edit.Command, TermTaxonomy>()
                .ForMember(d => d.CreatedDate, o => o.Ignore())
                .ForPath(d => d.Term.Id, o => o.Ignore())
                .ForPath(d => d.Term.Name, o => o.MapFrom(s => s.Term.Name))
                .ForPath(d => d.Term.Slug, o => o.MapFrom(s => s.Term.Slug));
        }
    }
}