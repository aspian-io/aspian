using System;
using Aspian.Domain.BaseModel;

namespace Aspian.Domain.UserModel
{
    public interface IUserToken : IEntityCreate, IEntityInfo
    {
        string RefreshToken { get; set; }
        DateTime ExpiresAt { get; set; }
        public bool IsExpired { get; }
        DateTime? RevokedAt { get; set; }
        public bool IsActive { get; }
        string ReplacedByToken { get; set; }
    }
}