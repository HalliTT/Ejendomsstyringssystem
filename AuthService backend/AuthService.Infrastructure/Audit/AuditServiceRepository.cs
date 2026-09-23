using AuthService.Application.Audit;
using AuthService.Domain.Audit;
using AuthService.Infrastructure.Persistence;

namespace AuthService.Infrastructure.Audit
{
    public sealed class AuditServiceRepository : IAuditService
    {
        private readonly AuthDbContext _db;
        private readonly IRequestContext _context;

        public AuditServiceRepository(AuthDbContext db, IRequestContext context)
        {
            _db = db;
            _context = context;
        }

        public async Task SaveAsync(AuthEvents authEvent)
        {
            try
            {
                _db.AuthEvents.Add(authEvent);
                await _db.SaveChangesAsync();
            }
            catch
            {

            }
        }

        public async Task LogSuccessAsync(AuthEventTypes eventType)
        {
            await SaveAsync(new AuthEvents
            {
                EventType = eventType,
                Severity = Severity.INFO,
                Success = true,
                UserId = _context.UserId,
                IpAddress = _context.IpAddress,
                UserAgent = _context.UserAgent
            });
        }

        public async Task LogFailureAsync(AuthEventTypes eventType)
        {
            await SaveAsync(new AuthEvents
            {
                EventType = eventType,
                Severity = MapSeverity(eventType),
                Success = false,
                UserId = _context.UserId,
                IpAddress = _context.IpAddress,
                UserAgent = _context.UserAgent
            });
        }

        private Severity MapSeverity(AuthEventTypes eventType)
        {
            return eventType switch
            {
                AuthEventTypes.LOGIN_FAILURE => Severity.WARNING,
                AuthEventTypes.REFRESH_TOKEN_REUSE => Severity.CRITICAL,
                AuthEventTypes.ACCOUNT_LOCKED => Severity.CRITICAL,
                _ => Severity.WARNING
            };
        }
    }
}
