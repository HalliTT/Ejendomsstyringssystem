using AuthService.Application.Users;
using AuthService.Domain.Users;
using AuthService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Infrastructure.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext _db;
        public UserRepository(AuthDbContext db)
        {
            _db = db;
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _db.Users.SingleOrDefaultAsync(u => u.Id == id, ct);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct)
        {
            return await _db.Users.SingleOrDefaultAsync(u => u.Email == email, ct);
        }

        public async Task<IReadOnlyList<User>> GetUserAsync(CancellationToken ct)
        {
            return await _db.Users
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken ct)
        {
            return await _db.Users
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<User>> GetAllWithAddressAsync(CancellationToken ct)
        {
            return await _db.Users
                .AsNoTracking()
                .Include(u => u.Address)
                .ToListAsync(ct);
        }

    }
}
