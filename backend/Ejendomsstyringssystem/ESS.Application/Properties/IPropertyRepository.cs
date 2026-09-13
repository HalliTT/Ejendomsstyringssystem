using ESS.Domain.Properties;

namespace ESS.Application.Properties
{
    public interface IPropertyRepository
    {
        Task<IReadOnlyList<Property>> ListAsync(CancellationToken ct);
        Task<Property?> GetByIdAsync(Guid id, CancellationToken ct);

    }
}
