using ESS.Domain.Users;

namespace ESS.Application.Users
{
    public interface IUserProvisioningService
    {
        Task EnsureUserAsync(Guid userId, string? email, string? displayName, CancellationToken ct);
    }
}
