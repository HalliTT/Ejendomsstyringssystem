using AuthService.Application.Common;

namespace AuthService.Application.Organizations
{
    public sealed record OrganizationResult
    (
        Guid Id,
        string Name,
        string Slug,
        string? Description,
        Guid? OwnerId,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        DateTime? SoftDeletedAt = null,
        UserMinimalResult? DeletedByUser = null
    );
}
