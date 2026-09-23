using AuthService.Application.Auth.Grants;
using AuthService.Application.Common.Interfaces;
using AuthService.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Application.Auth.Session.GenerateSession
{
    public sealed class Handler : IRequestHandler<Command, Result<SessionResult>>
    {
        private readonly IGrantRepository _grantRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IFingerprintBuilder _fingerprintBuilder;
        private readonly IClock _clock;

        public Handler(
            IGrantRepository grantRepository,
            ISessionRepository sessionRepository,
            IFingerprintBuilder fingerprintBuilder,
            IClock clock)
        {
            _grantRepository = grantRepository;
            _sessionRepository = sessionRepository;
            _fingerprintBuilder = fingerprintBuilder;
            _clock = clock;
        }

        public async Task<Result<SessionResult>> Handle(Command request, CancellationToken ct)
        {
            var grant = await _grantRepository.GetByCodeAsyncWithLock(request.code, ct);
            if (grant == null)
                return Result<SessionResult>.Failure();

            var now = _clock.UtcNow;

            var fingerprint = _fingerprintBuilder.Build(request.deviceId);

            try
            {
                grant.EnsureValid(now);
                grant.ValidatePkce(request.codeVerifier);
                grant.Consume(now);

                var session = AuthService.Domain.Sessions.Session.Create(grant.UserId, fingerprint, now);

                var (accessRaw, refreshRaw, access, refresh) = session.IssueTokens(now, grant.Scopes);

                await _sessionRepository.AddAsync(session, ct);

                return Result<SessionResult>.Success(
                    new SessionResult(
                        accessRaw,
                        access.ExpiresAt,
                        refreshRaw,
                        refresh.ExpiresAt,
                        session.Id,
                        session.UserId));
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result<SessionResult>.Failure();
            }
            catch
            {
                return Result<SessionResult>.Failure();
            }
        }
    }
}
