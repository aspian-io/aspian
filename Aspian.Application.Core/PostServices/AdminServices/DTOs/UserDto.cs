using System.Collections.Generic;

namespace Aspian.Application.Core.PostServices.AdminServices.DTOs
{
    public class UserDto
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public string Role { get; set; }
        public PhotoDto ProfilePhoto { get; set; }
    }
}