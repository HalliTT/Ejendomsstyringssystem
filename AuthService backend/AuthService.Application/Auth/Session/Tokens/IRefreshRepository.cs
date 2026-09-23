using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AuthService.Application.Auth.Session.Tokens
{
    public interface IRefreshRepository
    {
        Task AddAsync(Domain.Tokens.RefreshToken refreshToken, CancellationToken ct);

    }
}
