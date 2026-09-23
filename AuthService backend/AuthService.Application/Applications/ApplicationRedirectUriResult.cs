namespace AuthService.Application.Applications
{
    public sealed record ApplicationRedirectUriResult
    (
        Guid Id,
        Guid ApplicationId,
        string RedirectUri,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );
}
