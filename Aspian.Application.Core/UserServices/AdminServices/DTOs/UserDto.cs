using System.Text.Json.Serialization;

namespace Aspian.Application.Core.UserServices.AdminServices.DTOs
{
    public class UserDto
    {
        public string DisplayName { get; set; }
        public string Token { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string ProfilePhotoName { get; set; }
    }
}