using ESS.Domain.Units;

namespace ESS.Application.Units
{
    public interface IUnitRepository
    {
        Task<IReadOnlyList<Unit>> ListAsync(CancellationToken ct);

        Task<IReadOnlyList<Unit>> ListByPropertyIdAsync(Guid propertyId, CancellationToken ct);
    }
}
