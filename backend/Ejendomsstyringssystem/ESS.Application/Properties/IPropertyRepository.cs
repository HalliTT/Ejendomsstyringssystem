using ESS.Application.Properties.List;
using ESS.Domain.Properties;
using ESS.Domain.Units;

namespace ESS.Application.Properties
{
    public interface IPropertyRepository
    {
        Task<IReadOnlyList<Property>> ListAsync(CancellationToken ct);
        Task<Property?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<Property?> CreateAsync(string name, string address, string city, string country, string description, CancellationToken ct);
    }
}
