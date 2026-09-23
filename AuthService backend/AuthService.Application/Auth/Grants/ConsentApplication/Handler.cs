using AuthService.Application.Audit;
using AuthService.Application.Auth.Grants;
using AuthService.Application.Auth.Login;
using AuthService.Application.Common.Interfaces;
using AuthService.Application.Organizations;
using AuthService.Application.Services;
using AuthService.Domain.Audit;
using AuthService.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Auth.Grants.ConsentApplication
{
    public sealed class Handler : IRequestHandler<Command, Result<ConsentResult>>
    {
        private readonly IGrantRepository _repo;
        private readonly IClock _clock;
        private readonly IMembershipService _membershipService;
        private readonly IAuditService _audit;

        public Handler(IGrantRepository repo, IClock clock, IMembershipService membershipService, IAuditService auditService)
        {
            _repo = repo;
            _clock = clock;
            _membershipService = membershipService;
            _audit = auditService;
        }

        public async Task<Result<ConsentResult>> Handle(Command request, CancellationToken ct)
        {
            var grant = await _repo.GetByIdAsync(request.GrantId, ct)
                ?? throw new Exception("Grant not found");

            var now = _clock.UtcNow;

            grant.Approve(now);

            string query;

            if (request.Approved)
            {
                query = grant.ApproveConsent(now);

                await _audit.LogFailureAsync(AuthEventTypes.CONSENT_APPROVED);
                var orgId = grant.Application?.OrganizationId;
                if (orgId != null && orgId != Guid.Empty)
                {
                    await _membershipService.EnsureActiveMembership(
                        grant.UserId,
                        orgId.Value,
                        ct);
                }
            }
            else
            {
                query = grant.DenyConsent(now);
            }
            await _repo.SaveAsync(ct);

            var redirectUrl = $"{grant.RedirectUri}?{query}";

            return Result<ConsentResult>.Success(new ConsentResult(redirectUrl));
        }

    }
}
