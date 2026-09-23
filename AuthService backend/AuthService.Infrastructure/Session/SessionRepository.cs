using AuthService.Application.Auth.Session;
using AuthService.Domain.Tokens;
using AuthService.Domain.Users;
using AuthService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Session
{
    public sealed class SessionRepository : ISessionRepository
    {
        private readonly AuthDbContext _db;

        public SessionRepository(AuthDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(AuthService.Domain.Sessions.Session session, CancellationToken ct)
        {
            _db.Session.Add(session);
            await _db.SaveChangesAsync(ct);
        }
        public async Task UpdateAsync(AuthService.Domain.Sessions.Session session, CancellationToken ct)
        {
            _db.Session.Update(session);
            await _db.SaveChangesAsync(ct);
        }

        public async Task SaveAsync(CancellationToken ct)
        {
            await _db.SaveChangesAsync(ct);
        }

        public async Task<RefreshToken?> GetRefreshTokenWithSessionLock(string tokenHash, CancellationToken ct)
        {
            return await _db.RefreshTokens
                .Include(x => x.Session)
                    .ThenInclude(s => s.RefreshTokens)
                .Include(x => x.Session)
                    .ThenInclude(s => s.AccessTokens)
                .Where(x => x.TokenHash == tokenHash)
                .FirstOrDefaultAsync(ct);
        }

        public async Task<AuthService.Domain.Sessions.Session?> GetSessionById(Guid sessionId, CancellationToken ct)
        {
            return await _db.Session.SingleOrDefaultAsync(s => s.Id == sessionId, ct);
        }

        public async Task<List<AuthService.Domain.Sessions.Session>> GetActiveByUser(Guid userId, CancellationToken ct)
        {
            return await _db.Session
               .AsNoTracking()
               .Where(x => x.UserId == userId)
               .ToListAsync(ct);
        }
    }
}
