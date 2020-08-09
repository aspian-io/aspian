using System;
using System.Text.Json.Serialization;

namespace Aspian.Application.Core.UserServices.AdminServices.DTOs
{
    public class RefreshTokenDto
    {
        public string JWT { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; }
    }
}