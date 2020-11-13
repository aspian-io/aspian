using System.IO;
using System.Security.Cryptography;

namespace Infrastructure.Upload.Tus.Extensions.Internal
{
    internal static class FileStreamExtensions
    {
        public static byte[] CalculateSha1(this Stream stream, long chunkStartPosition)
        {
            byte[] fileHash;
            using (var sha1 = SHA1.Create())
            {
                var originalPos = stream.Position;
                stream.Seek(chunkStartPosition, SeekOrigin.Begin);
                fileHash = sha1.ComputeHash(stream);
                stream.Seek(originalPos, SeekOrigin.Begin);
            }

            return fileHash;
        }

    }
}