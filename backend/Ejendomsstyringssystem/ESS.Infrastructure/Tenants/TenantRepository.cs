using ESS.Application.Tenants;
using ESS.Domain.Tenants;
using ESS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ESS.Infrastructure.Tenants
{
    public class TenantRepository : ITenantRepository
    {
        private readonly EssDbContext _context;

        public TenantRepository(EssDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Tenant>> ListAsync(CancellationToken ct)
        {
            return await _context.Tenants
                .AsNoTracking()
                .Where(t => t.SoftDeletedAt == null)
                .ToListAsync(ct);
        }

        public async Task<Tenant?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _context.Tenants
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id && t.SoftDeletedAt == null, ct);
        }
    }
}
