using ESS.Application.Bookings.Get;

namespace ESS.Application.Bookings
{
    public sealed record BookingMutationResult(BookingDto? Booking, bool NotFound, bool HasOverlap);
}
