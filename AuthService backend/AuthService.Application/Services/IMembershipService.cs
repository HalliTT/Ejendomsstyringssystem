using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Services
{
    public interface IMembershipService
    {
        Task EnsureActiveMembership(Guid userId, Guid organizationId, CancellationToken ct);
    }
}
