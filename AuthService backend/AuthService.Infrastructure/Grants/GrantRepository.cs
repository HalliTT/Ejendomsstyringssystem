using AuthService.Domain.Grants;
using AuthService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using AuthService.Application.Auth.Grants;

namespace AuthService.Infrastructure.Grants
{
    public sealed class GrantRepository : IGrantRepository
    {
        private readonly AuthDbContext _db;

        public GrantRepository(AuthDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Grant grant, CancellationToken ct)
        {
            _db.Grants.Add(grant);
            await _db.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(Grant grant, CancellationToken ct)
        {
            _db.Grants.Update(grant);
            await _db.SaveChangesAsync(ct);
        }

        public async Task<Grant?> GetByCodeAsyncWithLock(string code, CancellationToken ct)
        {
            return await _db.Grants
                .AsTracking()
                .Where(g => g.CodeHash == code && g.RevokedAt == null && g.ExpiresAt > DateTime.UtcNow && g.ConsumedAt == null)
                .FirstOrDefaultAsync(ct);
        }

        public async Task<Grant?> GetByIdAsync(Guid? id, CancellationToken ct)
        {
            return await _db.Grants
                .Include(g => g.Application)
                .SingleOrDefaultAsync(g => g.Id == id, ct);
        }

        public async Task<IReadOnlyList<Grant>> GetByApplicationIdAsync(Guid applicationId, CancellationToken ct)
        {
            return await _db.Grants
                .AsNoTracking()
                .Where(g => g.ApplicationId == applicationId && g.RevokedAt == null && g.ExpiresAt > DateTime.UtcNow)
                .ToListAsync(ct);
        }

        public async Task SaveAsync(CancellationToken ct)
        {
            await _db.SaveChangesAsync(ct);
        }
    }
}
