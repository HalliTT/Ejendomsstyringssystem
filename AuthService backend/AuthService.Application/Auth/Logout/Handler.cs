using AuthService.Application.Auth.Session;
using AuthService.Application.Common.Interfaces;
using AuthService.Domain.Common;
using MediatR;

namespace AuthService.Application.Auth.Logout
{
    public sealed class Handler : IRequestHandler<Command, Result>
    {
        private readonly ISessionRepository _sessions;
        private readonly IClock _clock;
        public Handler(ISessionRepository sessions, IClock clock)
        {
            _sessions = sessions;
            _clock = clock;
        }
        public async Task<Result> Handle(Command request, CancellationToken ct)
        {
            var session = await _sessions.GetSessionById(request.SessionId, ct);

            if (session == null)
                return Result.Success();

            session.Revoke(_clock.UtcNow);

            await _sessions.UpdateAsync(session, ct);

            return Result.Success();
        }
    }
}
