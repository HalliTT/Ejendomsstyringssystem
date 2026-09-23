using ESS.Domain.Rentals;

namespace ESS.Application.Rentals
{
    public interface IRentalOptionRepository
    {
        Task<IReadOnlyList<RentalOption>> ListByPropertyIdAsync(Guid propertyId, CancellationToken ct);
        Task<IReadOnlyList<RentalOption>> ListByOwnerIdAsync(Guid ownerId, CancellationToken ct);
        Task<IReadOnlyDictionary<Guid, List<Guid>>> GetUnitIdsByRentalOptionIdsAsync(IReadOnlyList<Guid> rentalOptionIds, CancellationToken ct);
        Task<RentalOption?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<RentalOption?> CreateAsync(string name, decimal monthlyRent, RentalOptionStatus status, Guid propertyId, IReadOnlyList<Guid> unitIds, CancellationToken ct);
        Task<RentalOption?> UpdateAsync(Guid id, string name, decimal monthlyRent, RentalOptionStatus status, IReadOnlyList<Guid> unitIds, CancellationToken ct);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct);
    }
}
