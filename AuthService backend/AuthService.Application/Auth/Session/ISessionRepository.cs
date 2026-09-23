using AuthService.Domain.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Auth.Session
{
    public interface ISessionRepository
    {
        Task AddAsync(Domain.Sessions.Session session, CancellationToken ct);
        Task UpdateAsync(Domain.Sessions.Session session, CancellationToken ct);
        Task SaveAsync(CancellationToken ct);

        Task<Domain.Sessions.Session?> GetSessionById(Guid sessionId, CancellationToken ct);
        Task<List<Domain.Sessions.Session>> GetActiveByUser(Guid userId, CancellationToken ct);

        Task<RefreshToken?> GetRefreshTokenWithSessionLock(string tokenHash, CancellationToken ct);
    }
}
