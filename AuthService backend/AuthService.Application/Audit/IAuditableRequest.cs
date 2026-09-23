using AuthService.Domain.Audit;

namespace AuthService.Application.Audit
{
    public interface IAuditableRequest
    {
        AuthEventTypes? EventType { get; }
    }
}
