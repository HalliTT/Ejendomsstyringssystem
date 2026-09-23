using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Applications
{
    public interface IApplicationRepository
    {
        Task<ApplicationResult?> GetByClientIdAsync(Guid clientId, CancellationToken ct);
        Task<IReadOnlyList<ApplicationResult>> ListByOrganizationAsync(Guid organizationId, CancellationToken ct);
        Task<ApplicationResult> CreateAsync(Guid organizationId, string name, IReadOnlyList<string> redirectUris, DateTime now, CancellationToken ct);
        Task<ApplicationResult?> UpdateAsync(Guid organizationId, Guid applicationId, string name, bool isEnabled, DateTime now, CancellationToken ct);
        Task<bool> SoftDeleteAsync(Guid organizationId, Guid applicationId, Guid deletedBy, DateTime now, CancellationToken ct);
        Task<IReadOnlyList<ApplicationRedirectUriResult>> ListRedirectUrisAsync(Guid applicationId, CancellationToken ct);
        Task<bool> AddRedirectUriAsync(Guid applicationId, string redirectUri, DateTime now, CancellationToken ct);
        Task<bool> DeleteRedirectUriAsync(Guid applicationId, Guid redirectUriId, CancellationToken ct);
    }
}
