using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AuthService.Application.Auth.Session.Tokens
{
    public interface IAccessRepository
    {
        Task AddAsync(Domain.Tokens.AccessToken accessToken, CancellationToken ct);
        Task<Domain.Tokens.AccessToken?> GetActiveWithSessionByRawTokenAsync(string rawToken, DateTime now, CancellationToken ct);
    }
}
