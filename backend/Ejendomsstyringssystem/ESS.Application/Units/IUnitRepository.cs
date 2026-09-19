using ESS.Domain.Units;

namespace ESS.Application.Units
{
    public interface IUnitRepository
    {
        Task<IReadOnlyList<Unit>> ListAsync(CancellationToken ct);

        Task<IReadOnlyList<Unit>> ListByPropertyIdAsync(Guid propertyId, CancellationToken ct);

        Task<Unit?> GetByIdAsync(Guid id, CancellationToken ct);

        Task<Unit?> CreateAsync(string name, string description, UnitStatus status, Guid propertyId, CancellationToken ct);
        Task<Unit?> UpdateAsync(Guid id, string name, string description, UnitStatus status, CancellationToken ct);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct);
    }
}
