using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Contracts.Auth
{
    public sealed record VerifyRequest(
        Guid ClientId,
        string RedirectUri,
        string ResponseType,
        string CodeChallenge,
        string CodeChallengeMethod,
        string State,
        string? Scopes
    );
}
