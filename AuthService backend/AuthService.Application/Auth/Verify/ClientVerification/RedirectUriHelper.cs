using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Auth.Verify.ClientVerification
{
    internal static class RedirectUriHelper
    {
        public static string Normalize(string uri)
        {
            if (!Uri.TryCreate(uri, UriKind.Absolute, out var u))
                return uri.Trim();

            var port = u.IsDefaultPort ? "" : $":{u.Port}";
            var path = string.IsNullOrEmpty(u.AbsolutePath) ? "/" : u.AbsolutePath.TrimEnd('/');
            return $"{u.Scheme}://{u.Host}{port}{path}";
        }
    }
}
