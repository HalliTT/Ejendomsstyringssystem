namespace AuthService.Application.Audit
{
    public interface IAuditRepository
    {
        Task<List<AuditEventResult>> GetRecentEventsAsync(int take, CancellationToken ct);
    }
}
