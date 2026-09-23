using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AuthService.Domain.Common
{
    public static class PkceVerifier
    {
        public static string Compute(string verifier, string method)
        {
            if (method != "S256")
                throw new NotSupportedException("Only S256 supported");

            var bytes = Encoding.UTF8.GetBytes(verifier);

            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(bytes);

            return Convert.ToBase64String(hash)
                .Replace('+', '-')
                .Replace('/', '_')
                .TrimEnd('=');
        }
    }
}
