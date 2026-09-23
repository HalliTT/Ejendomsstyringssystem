using AuthService.Application.Audit;
using AuthService.Application.Common;
using AuthService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Audit
{
    public sealed class AuditRepository : IAuditRepository
    {
        private readonly AuthDbContext _db;

        public AuditRepository(AuthDbContext db)
        {
            _db = db;
        }

        public async Task<List<AuditEventResult>> GetRecentEventsAsync(int take, CancellationToken ct)
        {
            var events = await _db.AuthEvents
                .OrderByDescending(x => x.CreatedAt)
                .Take(take)
                .GroupJoin(
                    _db.Users,
                    evt => evt.UserId,
                    usr => usr.Id,
                    (evt, users) => new { Event = evt, User = users.FirstOrDefault() }
                )
                .GroupJoin(
                    _db.Applications,
                    x => x.Event.ClientId,
                    app => app.Id,
                    (x, apps) => new
                    {
                        x.Event,
                        x.User,
                        Application = apps.FirstOrDefault()
                    }
                )
                .Select(x => new AuditEventResult(
                    x.Event.Id,
                    x.Event.EventType,
                    x.Event.Severity,
                    x.Event.UserId,
                    x.Event.ClientId,
                    x.Event.Email,
                    x.Event.IpAddress != null ? x.Event.IpAddress.ToString() : null,
                    x.Event.UserAgent,
                    x.Event.Success,
                    x.Event.ErrorCode,
                    x.Event.ErrorMessage,
                    x.Event.CreatedAt,
                    x.User != null
                        ? new UserMinimalResult(
                            x.User.Id,
                            x.User.FirstName,
                            x.User.LastName,
                            x.User.DisplayName
                        )
                        : null,
                    x.Application != null
                        ? new ApplicationAuditResult(
                            x.Application.Id,
                            x.Application.Name,
                            x.Application.ClientId.ToString()
                        )
                        : null
                ))
                .ToListAsync(ct);

            return events;
        }
    }
}
