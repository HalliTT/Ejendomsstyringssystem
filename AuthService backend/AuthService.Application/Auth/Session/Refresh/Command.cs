using AuthService.Domain.Common;
using MediatR;

namespace AuthService.Application.Auth.Session.Refresh
{
    public sealed record Command
    (
        string RefreshToken,
        Guid ClientId,
        string RedirectUri,
        string CodeChallenge,
        string CodeChallengeMethod,
        string Scopes
    ) : IRequest<Result<RefreshResult>>;
}
