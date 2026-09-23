namespace AuthService.Contracts.Management
{
    public sealed record CreateApplicationRequest
    (
        string Name,
        IReadOnlyList<string> RedirectUris
    );
}
