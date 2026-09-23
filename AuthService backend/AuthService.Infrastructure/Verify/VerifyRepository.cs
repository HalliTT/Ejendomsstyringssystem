using AuthService.Application.Auth.Verify;
using AuthService.Application.Auth.Verify.ClientVerification;
using AuthService.Domain.Users;
using AuthService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Infrastructure.Verify
{
    public class VerifyRepository : IVerifyRepository
    {
        private readonly AuthDbContext _db;
        public VerifyRepository(AuthDbContext db)
        {
            _db = db;
        }

        public async Task<ClientResult?> GetByClientIdAsync(Guid clientId,CancellationToken ct)
        {
            var result = await _db.Applications
                .AsNoTracking()
                .Where(a =>
                    a.ClientId == clientId &&
                    a.IsEnabled == true &&
                    a.SoftDeletedAt == null)
                .Select(a => new ClientResult(
                    a.Id,
                    a.ClientId,
                    a.IsEnabled,
                    a.SoftDeletedAt,
                    a.ApplicationRedirectUrises.Select(aru => aru.RedirectUris ?? string.Empty).ToList()
                ))
                .SingleOrDefaultAsync(ct);
            return result;
        }
    }
}
