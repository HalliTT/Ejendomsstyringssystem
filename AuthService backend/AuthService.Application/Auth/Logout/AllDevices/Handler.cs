using AuthService.Domain.Common;
using MediatR;
using AuthService.Application.Auth.Session;
using AuthService.Application.Common.Interfaces;

namespace AuthService.Application.Auth.Logout.AllDevices
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
            var sessions = await _sessions.GetActiveByUser(request.UserId, ct);

            var now = _clock.UtcNow;

            foreach (var session in sessions)
                session.Revoke(now);

            await _sessions.SaveAsync(ct);

            return Result.Success();
        }
    }
}
