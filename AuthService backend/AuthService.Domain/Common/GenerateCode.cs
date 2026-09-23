using System.Security.Cryptography;
using System.Text;

namespace AuthService.Domain.Common
{
    public class GenerateCode
    {
        public static (string raw, string hash) GenerateSecureCode(int numberBytes)
        {
            var bytes = RandomNumberGenerator.GetBytes(numberBytes);

            var raw = Convert.ToBase64String(bytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .TrimEnd('=');

            var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(raw));

            var hash = Convert.ToBase64String(hashBytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .TrimEnd('=');

            return (raw, hash);
        }

        public static string HashToken(string raw)
        {
            var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(raw));

            return Convert.ToBase64String(hashBytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .TrimEnd('=');
        }

        public static bool ValidateCode(string raw, string expectedHash)
        {
            var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(raw));

            var hash = Convert.ToBase64String(hashBytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .TrimEnd('=');

            return hash == expectedHash;
        }
    }
}
