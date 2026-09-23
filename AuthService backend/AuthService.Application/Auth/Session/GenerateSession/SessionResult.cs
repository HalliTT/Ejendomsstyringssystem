namespace AuthService.Application.Auth.Session.GenerateSession
{
    public sealed record SessionResult
    (
        string AccessToken,
        DateTime AccessTokenExpires,
        string RefreshToken,
        DateTime RefreshTokenExpires,
        Guid SessionId,
        Guid UserId
    );
}
