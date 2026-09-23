using ESS.Application.Users;
using ESS.Domain.Users;
using ESS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESS.Infrastructure.Users
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly EssDbContext _context;

        public AppUserRepository(EssDbContext context)
        {
            _context = context;
        }

        public async Task<AppUser?> GetByIdAsync(Guid id, CancellationToken ct)
        {
           return await _context.AppUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id, ct);
        }
    }
}