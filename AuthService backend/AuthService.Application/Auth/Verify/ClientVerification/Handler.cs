using AuthService.Application.Applications;
using AuthService.Application.Auth.Grants;
using AuthService.Application.Common.Interfaces;
using AuthService.Domain.Common;
using AuthService.Domain.Grants;
using MediatR;
using System;

namespace AuthService.Application.Auth.Verify.ClientVerification
{
    public sealed class Handler : IRequestHandler<Command, Result>
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IClock _clock;
        private readonly IGrantRepository _grantRepository;
        private readonly IAuthorizationRequestStore _authStore;


        public Handler(IApplicationRepository applicationRepository, IClock clock, IGrantRepository grantRepository, IAuthorizationRequestStore authorizationRequestStore)
        {
            _applicationRepository = applicationRepository;
            _clock = clock;
            _grantRepository = grantRepository;
            _authStore = authorizationRequestStore;
        }

        public async Task<Result> Handle(
            Command request, 
            CancellationToken ct)
        {

            var client = await _applicationRepository.GetByClientIdAsync(request.ClientId, ct);

            if (client is null)
                return Result.Failure();

            if (!client.IsEnabled)
                return Result.Failure();

            if (client.SoftDeletedAt != null)
                return Result.Failure();

            if (!string.IsNullOrEmpty(request.RedirectUri))
            {
                var normalizedRequest = RedirectUriHelper.Normalize(request.RedirectUri);
                var match = client.RedirectUris
                    .Select(RedirectUriHelper.Normalize)
                    .Any(uri => uri == normalizedRequest);

                if (!match)
                    return Result.Failure();
            }
            else
            {
                return Result.Failure();
            }

            var grants = await _grantRepository.GetByApplicationIdAsync(client.Id, ct);

            var duplicate = grants.Any(g => g.CodeChallange == request.CodeChallenge);
            if (duplicate)
                return Result.Failure();

            var newGrant = Grant.CreateAuthorizationCode(
                    client.Id,
                    request.RedirectUri,
                    request.CodeChallenge,
                    request.CodeChallengeMethod,
                    _clock.UtcNow);

            // remember requested scopes for later consent checks
            newGrant.Scopes = request.Scopes;

            await _grantRepository.AddAsync(newGrant, ct);

            _authStore.SetGrantIt(newGrant.Id);

            return Result.Success();
        }
    }
}
