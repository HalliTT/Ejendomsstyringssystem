using AuthService.Application.Auth.Session.Tokens;
using AuthService.Domain.Tokens;
using AuthService.Infrastructure.Persistence;

namespace AuthService.Infrastructure.Tokens
{
    public sealed class RefreshRepository : IRefreshRepository
    {
        private readonly AuthDbContext _db;

        public RefreshRepository(AuthDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(RefreshToken refreshToken, CancellationToken ct)
        {
            _db.RefreshTokens.Add(refreshToken);
            await _db.SaveChangesAsync(ct);
        }
    }
}
