using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Contracts.Auth
{
    public sealed record RefreshRequest
    (
        string RefreshToken,
        Guid ClientId,
        string RedirectUri,
        string CodeChallenge,
        string CodeChallengeMethod,
        string Scopes
    );
}
