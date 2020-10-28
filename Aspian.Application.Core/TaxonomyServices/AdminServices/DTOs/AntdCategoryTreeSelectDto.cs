using System.Collections.Generic;

namespace Aspian.Application.Core.TaxonomyServices.AdminServices.DTOs
{
    public class AntdCategoryTreeSelectDto
    {
        public string Title { get; set; }
        public string Value { get; set; }
        public string Key { get; set; }
        public ICollection<AntdCategoryTreeSelectDto> Children { get; set; }
    }
}