using ESS.Application.Properties.List;
using ESS.Domain.Properties;
using ESS.Domain.Units;

namespace ESS.Application.Properties
{
    public interface IPropertyRepository
    {
        Task<IReadOnlyList<Property>> ListAsync(Guid ownerId, CancellationToken ct);
        Task<Property?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<Property?> CreateAsync(Guid ownerId,string name, string address, string city, string country, string description, CancellationToken ct);
        Task<Property?> UpdateAsync(Guid id, string name, string address, string city, string country, string description, CancellationToken ct);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct);
    }
}
