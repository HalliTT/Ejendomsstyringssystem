using AuthService.Domain.Audit;

namespace AuthService.Application.Audit
{
    public interface IAuditService
    {
        Task SaveAsync(AuthEvents authEvent);
        Task LogSuccessAsync(AuthEventTypes eventType);
        Task LogFailureAsync(AuthEventTypes eventType);
    }
}
