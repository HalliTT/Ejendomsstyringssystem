using AuthService.Application.Common;
using AuthService.Domain.Audit;

namespace AuthService.Application.Audit
{
    public sealed record AuditEventResult(
        Guid Id,
        AuthEventTypes EventType,
        Severity Severity,
        Guid? UserId,
        Guid? ClientId,
        string? Email,
        string? IpAddress,
        string? UserAgent,
        bool Success,
        string? ErrorCode,
        string? ErrorMessage,
        DateTime CreatedAt,
        UserMinimalResult? User = null,
        ApplicationAuditResult? Application = null
    );

    public sealed record UserAuditResult(
        Guid Id,
        string? FirstName,
        string? LastName,
        string? DisplayName
    );

    public sealed record ApplicationAuditResult(
        Guid Id,
        string? Name,
        string ClientId
    );
}
