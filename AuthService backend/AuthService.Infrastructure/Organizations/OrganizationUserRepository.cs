using AuthService.Application.Organizations;
using AuthService.Domain.Organizations;
using AuthService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Infrastructure.Organizations
{
    public sealed class OrganizationUserRepository : IOrganizationUserRepository
    {
        private readonly AuthDbContext _db;

        public OrganizationUserRepository(AuthDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(OrganizationUsers orgUser, CancellationToken ct)
        {
            _db.OrganizationUsers.Add(orgUser);
            await _db.SaveChangesAsync(ct);
        }

        public async Task<OrganizationUsers?> GetByUserAndOrganizationAsync(Guid userId, Guid organizationId, CancellationToken ct)
        {
            return await _db.OrganizationUsers
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.UserId == userId && x.OrganizationId == organizationId, ct);
        }

        public async Task UpdateAsync(OrganizationUsers orgUser, CancellationToken ct)
        {
            _db.OrganizationUsers.Update(orgUser);
            await _db.SaveChangesAsync(ct);
        }

        public async Task SaveChangesAsync(CancellationToken ct)
        {
            await _db.SaveChangesAsync(ct);
        }
    }
}
