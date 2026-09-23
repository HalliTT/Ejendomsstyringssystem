using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Domain.Grants
{
    public static class GrantErrors
    {
        public const string InvalidPkceChallenge = "PKCE challenge is invalid.";
        public const string UnsupportedPkceMethod = "PKCE method is not supported.";
        public const string GrantExpired = "Grant has expired.";
        public const string GrantRevoked = "Grant has been revoked.";
        public const string InvalidRedirectUri = "Redirect URI is invalid.";
    }
}
