using Aspian.Domain.BaseModel;

namespace Aspian.Application.Core.TaxonomyService
{
    public class TermDto : Entitymeta
    {
        public string Name { get; set; }
        public string Slug { get; set; }
    }
}