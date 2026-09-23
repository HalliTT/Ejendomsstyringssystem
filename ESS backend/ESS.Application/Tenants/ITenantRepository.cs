using ESS.Domain.Tenants;

namespace ESS.Application.Tenants
{
    public interface ITenantRepository
    {
        Task<IReadOnlyList<Tenant>> ListAsync(CancellationToken ct);
        Task<Tenant?> GetByIdAsync(Guid id, CancellationToken ct);
    }
}
