using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AuthService.Domain.Common
{
    public static class SecurityHash
    {
        public static string Sha256(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha.ComputeHash(bytes);

            return Convert.ToHexString(hash);
        }
    }
}
