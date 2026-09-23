using AuthService.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Organizations
{
    public interface IOrganizationUserRepository
    {
        Task<OrganizationUsers?> GetByUserAndOrganizationAsync(Guid userId, Guid organizationId, CancellationToken ct);
        Task AddAsync(OrganizationUsers orgUser, CancellationToken ct);
        Task UpdateAsync(OrganizationUsers orgUser, CancellationToken ct);
        Task SaveChangesAsync(CancellationToken ct);
    }
}
