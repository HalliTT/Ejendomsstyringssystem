using AuthService.Application.Auth.Verify.ClientVerification;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Auth.Verify
{
    public interface IVerifyRepository
    {
        Task<ClientResult?> GetByClientIdAsync(Guid clientId, CancellationToken ct);
    }
}
