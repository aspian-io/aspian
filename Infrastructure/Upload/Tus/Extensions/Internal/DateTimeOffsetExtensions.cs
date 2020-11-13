using System;

namespace Infrastructure.Upload.Tus.Extensions.Internal
{
    internal static class DateTimeOffsetExtensions
    {
        internal static bool HasPassed(this DateTimeOffset dateTime)
        {
            return dateTime.ToUniversalTime().CompareTo(DateTimeOffset.UtcNow) == -1;
        }
    }
}