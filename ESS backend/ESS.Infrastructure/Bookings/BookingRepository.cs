using ESS.Application.Bookings;
using ESS.Domain.Bookings;
using ESS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ESS.Infrastructure.Bookings
{
    public class BookingRepository : IBookingRepository
    {
        private readonly EssDbContext _context;

        public BookingRepository(EssDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Booking>> ListByOwnerIdAsync(Guid ownerId, CancellationToken ct)
        {
            var query =
                from booking in _context.Bookings
                join rentalOption in _context.RentalOptions on booking.RentalOptionId equals rentalOption.Id
                join property in _context.Properties on rentalOption.PropertyId equals property.Id
                where property.OwnerId == ownerId
                    && booking.SoftDeletedAt == null
                    && rentalOption.SoftDeletedAt == null
                select booking;

            return await query.AsNoTracking().ToListAsync(ct);
        }

        public async Task<Booking?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _context.Bookings
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id && b.SoftDeletedAt == null, ct);
        }

        public async Task<bool> HasOverlapAsync(Guid rentalOptionId, DateTime startDate, DateTime endDate, Guid? excludeBookingId, CancellationToken ct)
        {
            startDate = AsUtc(startDate);
            endDate = AsUtc(endDate);

            return await _context.Bookings
                .Where(b => b.RentalOptionId == rentalOptionId
                    && b.SoftDeletedAt == null
                    && (excludeBookingId == null || b.Id != excludeBookingId)
                    && b.StartDate < endDate
                    && b.EndDate > startDate)
                .AnyAsync(ct);
        }

        public async Task<Booking?> CreateAsync(Guid rentalOptionId, Guid tenantId, DateTime startDate, DateTime endDate, CancellationToken ct)
        {
            var booking = new Booking
            {
                RentalOptionId = rentalOptionId,
                TenantId = tenantId,
                StartDate = AsUtc(startDate),
                EndDate = AsUtc(endDate),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync(ct);

            return booking;
        }

        public async Task<Booking?> UpdateAsync(Guid id, Guid rentalOptionId, Guid tenantId, DateTime startDate, DateTime endDate, CancellationToken ct)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id && b.SoftDeletedAt == null, ct);
            if (booking is null)
                return null;

            booking.RentalOptionId = rentalOptionId;
            booking.TenantId = tenantId;
            booking.StartDate = AsUtc(startDate);
            booking.EndDate = AsUtc(endDate);
            booking.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(ct);
            return booking;
        }

        private static DateTime AsUtc(DateTime value) =>
            DateTime.SpecifyKind(value, DateTimeKind.Utc);

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id && b.SoftDeletedAt == null, ct);
            if (booking is null)
                return false;

            booking.SoftDeletedAt = DateTime.UtcNow;
            booking.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(ct);
            return true;
        }
    }
}
