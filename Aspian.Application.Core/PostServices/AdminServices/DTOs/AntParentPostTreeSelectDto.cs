using System.Collections.Generic;

namespace Aspian.Application.Core.PostServices.AdminServices.DTOs
{
    public class AntParentPostTreeSelectDto
    {
        public string Title { get; set; }
        public string Value { get; set; }
        public string Key { get; set; }
        public ICollection<AntParentPostTreeSelectDto> Children { get; set; }
    }
}