using System.Threading.Tasks;

namespace Infrastructure.Upload.Tus.Helpers
{
    internal static class TaskHelper
    {
        public static Task Completed { get; } = Task.FromResult(0);
    }
}