namespace AuthService.Contracts.Management
{
    public sealed record UpdateOrganizationRequest
    (
        string Name,
        string? Description,
        string Slug,
        Guid? OwnerId
    );
}
