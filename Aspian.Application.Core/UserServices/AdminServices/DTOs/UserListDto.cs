using System;
using System.Collections.Generic;

namespace Aspian.Application.Core.UserServices.AdminServices.DTOs
{
    public class UserListDto
    {
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public PhotoDto Photo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string UserAgent { get; set; }
        public string UserIPAddress { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public int Taxonomies { get; set; }
        public int Posts { get; set; }
        public int Attachments { get; set; }
        public int Comments { get; set; }
        public int Activites { get; set; }
    }
}