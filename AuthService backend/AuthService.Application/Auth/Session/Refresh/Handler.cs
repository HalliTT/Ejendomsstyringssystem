using AuthService.Application.Common.Interfaces;
using AuthService.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Auth.Session.Refresh
{
    public sealed class Handler : IRequestHandler<Command, Result<RefreshResult>>
    {
        private readonly ISessionRepository _sessions;
        private readonly IClock _clock;

        public Handler(ISessionRepository sessions, IClock clock)
        {
            _sessions = sessions;
            _clock = clock;
        }

        public async Task<Result<RefreshResult>> Handle(Command request, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                return Result<RefreshResult>.Failure();

            var now = _clock.UtcNow;

            //ToDo
            // Fingerprint validation
            // How to detect token theft vs normal IP change (Impossible travel)
            // How to model Device as its own aggregate
            // How to support multiple devices per session
            // How to revoke only ONE device session
            // How to enforce fingerprint validation on refresh
            // Logout all device endpoint
            // Clean up all Request/Commands and Result
            // Index strategy for 100M tokens
            // Error codes / when database is unavailable vs invalid token
            // Logout


            try
            {
                var hash = GenerateCode.HashToken(request.RefreshToken);

                var token = await _sessions.GetRefreshTokenWithSessionLock(hash, ct);

                if (token == null)
                    return Result<RefreshResult>.Failure();

                var session = token.Session;

                var (accessRaw, refreshRaw, access, refresh) =
                    session.RotateRefreshToken(token, now, request.Scopes);

                await _sessions.SaveAsync(ct);

                return Result<RefreshResult>.Success(
                    new RefreshResult(
                        accessRaw,
                        access.ExpiresAt,
                        refreshRaw,
                        refresh.ExpiresAt,
                        session.Id,
                        session.UserId));
            }
            catch (DbUpdateConcurrencyException)
            {
                // Another refresh request already rotated
                return Result<RefreshResult>.Failure();
            }
            catch
            {
                return Result<RefreshResult>.Failure();
            }

        }
    }
}
