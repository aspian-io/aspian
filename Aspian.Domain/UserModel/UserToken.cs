using System;
using Aspian.Domain.BaseModel;

namespace Aspian.Domain.UserModel
{
    public class UserToken : EntityCreate, IUserToken
    {
        public Guid Id { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public DateTime? RevokedAt { get; set; }
        public bool IsActive => (RevokedAt == null && !IsExpired) || (DateTime.UtcNow <= RevokedAt && !IsExpired);
        public string ReplacedByToken { get; set; }
        public string UserAgent { get; set; }
        public string UserIPAddress { get; set; }


    }
}