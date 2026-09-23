namespace AuthService.Application.Common
{
    /// <summary>
    /// Minimal user information - used for displaying who deleted a resource
    /// </summary>
    public sealed record UserMinimalResult(
        Guid Id,
        string? FirstName,
        string? LastName,
        string? DisplayName
    );
}
