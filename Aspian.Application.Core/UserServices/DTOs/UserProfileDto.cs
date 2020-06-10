using System.Collections.Generic;

namespace Aspian.Application.Core.UserServices.DTOs
{
    public class UserProfileDto
    {
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Image { get; set; }
        public string Bio { get; set; }
        public ICollection<PhotoDto> Photos { get; set; }
    }
}