namespace AuthService.Contracts.Auth
{
    public sealed record LogoutRequest
    (
        Guid sessionId
    );

    public sealed record LogoutRequestAll
    (
        Guid userId
    );
}
