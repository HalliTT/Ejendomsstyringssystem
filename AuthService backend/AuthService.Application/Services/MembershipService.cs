using AuthService.Application.Common.Interfaces;
using AuthService.Application.Organizations;
using AuthService.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AuthService.Application.Services
{
    public sealed class MembershipService : IMembershipService
    {
        private readonly IOrganizationUserRepository _repo;
        private readonly IClock _clock;

        public MembershipService(IOrganizationUserRepository repo, IClock clock)
        {
            _repo = repo;
            _clock = clock;
        }

        public async Task EnsureActiveMembership(Guid userId, Guid orgId, CancellationToken ct)
        {
            var existing = await _repo.GetByUserAndOrganizationAsync(userId, orgId, ct);

            if (existing != null)
            {
                existing.Activate(_clock.UtcNow);
            }
            else
            {
                var membership = OrganizationUsers.Create(userId, orgId, _clock.UtcNow);
                await _repo.AddAsync(membership, ct);
            }
        }
    }
}
