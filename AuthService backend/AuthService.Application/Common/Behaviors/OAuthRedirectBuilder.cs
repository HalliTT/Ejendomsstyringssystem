using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Common.Behaviors
{
    public static class OAuthRedirectBuilder
    {
        public static string Success(string redirectUri, string code)
        => $"{redirectUri}?code={Uri.EscapeDataString(code)}";

        public static string Error(string redirectUri, string error)
            => $"{redirectUri}?error={Uri.EscapeDataString(error)}";
    }
}
