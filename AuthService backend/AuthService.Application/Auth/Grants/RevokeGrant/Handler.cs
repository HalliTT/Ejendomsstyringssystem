using AuthService.Application.Auth.Grants;
using AuthService.Application.Auth.Grants.ConsentApplication;
using AuthService.Application.Common.Interfaces;
using AuthService.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Auth.Grants.RevokeGrant
{
    public sealed class Handler : IRequestHandler<Command, Result<ConsentResult>>
    {
        private readonly IGrantRepository _repo;
        private readonly IClock _clock;

        public Handler(IGrantRepository repo, IClock clock)
        {
            _repo = repo;
            _clock = clock;
        }

        public async Task<Result<ConsentResult>> Handle(Command request, CancellationToken ct)
        {
            var grant = await _repo.GetByIdAsync(request.GrantId, ct)
                ?? throw new Exception("Grant not found");

            grant.Revoke(_clock.UtcNow);

            await _repo.SaveAsync(ct);

            var redirectUrl = $"{grant.RedirectUri}?error=access_denied";

            return Result<ConsentResult>.Success(
               new ConsentResult(redirectUrl)
           );
        }
    }
}
