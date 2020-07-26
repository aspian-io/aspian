namespace Aspian.Application.Core.UserServices.UserServices.DTOs
{
    public class UserDto
    {
        public string DisplayName { get; set; }
        public string Token { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public PhotoDto Photo { get; set; }
    }
}