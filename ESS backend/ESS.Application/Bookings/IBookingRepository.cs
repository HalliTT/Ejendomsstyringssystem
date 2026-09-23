using ESS.Domain.Bookings;

namespace ESS.Application.Bookings
{
    public interface IBookingRepository
    {
        Task<IReadOnlyList<Booking>> ListByOwnerIdAsync(Guid ownerId, CancellationToken ct);
        Task<Booking?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<bool> HasOverlapAsync(Guid rentalOptionId, DateTime startDate, DateTime endDate, Guid? excludeBookingId, CancellationToken ct);
        Task<Booking?> CreateAsync(Guid rentalOptionId, Guid tenantId, DateTime startDate, DateTime endDate, CancellationToken ct);
        Task<Booking?> UpdateAsync(Guid id, Guid rentalOptionId, Guid tenantId, DateTime startDate, DateTime endDate, CancellationToken ct);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct);
    }
}
