using AuthService.Domain.Common;
using MediatR;

namespace AuthService.Application.Audit.GetRecentAuditEvents
{
    public sealed record Query(int Take = 10) : IRequest<Result<List<AuditEventResult>>>;
}
