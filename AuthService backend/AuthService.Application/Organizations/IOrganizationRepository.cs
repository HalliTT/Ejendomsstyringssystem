using AuthService.Contracts.Organization;
using AuthService.Domain.Grants;
using AuthService.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Organizations
{
    public interface IOrganizationRepository
    {
        Task<Organization?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<bool> IsUserInOrganizationAsync(Guid userId, Guid OrgId, CancellationToken ct);
        Task<IReadOnlyList<OrganizationResult>> ListAsync(CancellationToken ct);
        Task<OrganizationResult> CreateAsync(string name, string? description, string slug, Guid ownerId, Guid createdBy, DateTime now, CancellationToken ct);
        Task<OrganizationResult?> UpdateAsync(Guid organizationId, string name, string? description, string slug, Guid? ownerId, DateTime now, CancellationToken ct);
        Task<bool> SoftDeleteAsync(Guid organizationId, Guid deletedBy, DateTime now, CancellationToken ct);
        Task<bool> ExistsActiveAsync(Guid organizationId, CancellationToken ct);
        Task<bool> SlugExistsAsync(string slug, CancellationToken ct);
        Task<bool> SlugExistsForOtherAsync(Guid organizationId, string slug, CancellationToken ct);
    }
}
