using AuthService.Application.Auth.Session.Tokens;
using AuthService.Domain.Common;
using AuthService.Domain.Grants;
using AuthService.Domain.Tokens;
using AuthService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Infrastructure.Tokens
{
    public sealed class AccessRepository : IAccessRepository
    {
        private readonly AuthDbContext _db;

        public AccessRepository(AuthDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(AccessToken accessToken, CancellationToken ct)
        {
            _db.AccessTokens.Add(accessToken);
            await _db.SaveChangesAsync(ct);
        }

        public async Task<AccessToken?> GetActiveWithSessionByRawTokenAsync(string rawToken, DateTime now, CancellationToken ct)
        {
            var tokenHash = GenerateCode.HashToken(rawToken);

            return await _db.AccessTokens
                .Include(x => x.Session)
                .SingleOrDefaultAsync(x =>
                    (x.Token == tokenHash || x.Token == rawToken) &&
                    x.IsRevoked == false &&
                    x.ExpiresAt > now,
                    ct);
        }
    }
}
