using Aspian.Domain.PostModel;
using Aspian.Domain.TaxonomyModel;
using AutoMapper;

namespace Aspian.Application.Core.TaxonomyService
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TermTaxonomy, TaxonomyDto>();
            CreateMap<Term, TermDto>();
            CreateMap<Post, PostDto>();
            CreateMap<TermPost, TermPostDto>();
        }
    }
}