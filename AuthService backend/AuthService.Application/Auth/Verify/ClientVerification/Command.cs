using AuthService.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Auth.Verify.ClientVerification
{
    public sealed record Command
    (
        Guid ClientId,
        string RedirectUri,
        string ResponseType,
        string CodeChallenge,
        string CodeChallengeMethod,
        string State,
        string? Scopes
    ) : IRequest<Result>;
}
