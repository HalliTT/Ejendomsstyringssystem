using AuthService.Domain.Common;
using MediatR;

namespace AuthService.Application.Audit.GetRecentAuditEvents
{
    public sealed class Handler : IRequestHandler<Query, Result<List<AuditEventResult>>>
    {
        private readonly IAuditRepository _auditRepository;

        public Handler(IAuditRepository auditRepository)
        {
            _auditRepository = auditRepository;
        }

        public async Task<Result<List<AuditEventResult>>> Handle(Query request, CancellationToken ct)
        {
            if (request.Take <= 0 || request.Take > 100)
            {
                return Result<List<AuditEventResult>>.Failure();
            }

            var events = await _auditRepository.GetRecentEventsAsync(request.Take, ct);
            return Result<List<AuditEventResult>>.Success(events);
        }
    }
}
