using AuthService.Domain.Grants;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Auth.Grants
{
    public interface IGrantRepository
    {
        Task AddAsync(Grant grant, CancellationToken ct);

        Task UpdateAsync(Grant grant, CancellationToken ct);

        Task<Grant?> GetByCodeAsyncWithLock(string token, CancellationToken ct);

        Task<Grant> GetByIdAsync(Guid? id, CancellationToken ct);

        Task<IReadOnlyList<Grant>> GetByApplicationIdAsync(Guid applicationId, CancellationToken ct);

        Task SaveAsync(CancellationToken ct);
    }
}
