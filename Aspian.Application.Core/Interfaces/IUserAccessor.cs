namespace Aspian.Application.Core.Interfaces
{
    public interface IUserAccessor
    {
        string GetCurrentUsername();
        string GetCurrentUserIpAddress();
        string GetCurrentUserAgent();
    }
}