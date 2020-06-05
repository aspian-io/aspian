using Aspian.Domain.UserModel;

namespace Aspian.Application.Core.Interfaces
{
    public interface IJwtGenerator
    {
        string CreateToken(User user);
    }
}