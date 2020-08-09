using System.Collections.Generic;

namespace Aspian.Application.Core.UserServices.AdminServices.DTOs
{
    public class UserProfileDto
    {
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Bio { get; set; }
        public string Role { get; set; }
        public PhotoDto Photo { get; set; }
    }
}