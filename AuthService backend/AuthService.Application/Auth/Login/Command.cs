using AuthService.Application.Audit;
using AuthService.Domain.Audit;
using AuthService.Domain.Common;
using MediatR;

namespace AuthService.Application.Auth.Login
{
    public sealed record Command
    (
        Guid ClientId,
        string Email,
        string Password,
        string RedirectUri,
        string CodeChallenge,
        string CodeChallengeMethod,
        string Scopes
    ) : IRequest<Result<LoginResult>>;
}
