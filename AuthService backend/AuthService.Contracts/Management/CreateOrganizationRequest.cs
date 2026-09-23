namespace AuthService.Contracts.Management
{
    public sealed record CreateOrganizationRequest
    (
        string Name,
        string? Description,
        string Slug,
        Guid OwnerId
    );
}
