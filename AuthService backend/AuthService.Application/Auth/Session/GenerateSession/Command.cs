using AuthService.Domain.Common;
using MediatR;

namespace AuthService.Application.Auth.Session.GenerateSession
{
    public sealed record Command
    (
        //Guid ClientId,
        string code,
        string codeVerifier,
        string? deviceId
        //string? platform
        //string? screenResolution


        //string CodeVerifier,
        //string GrantType,
        //string RedirectUri,
        //string? refreshToken,

        //string? deviceId,
        //string? userAgent,
        //string? platform,
        //string? screenResolution

    ) : IRequest<Result<SessionResult>>;
}
