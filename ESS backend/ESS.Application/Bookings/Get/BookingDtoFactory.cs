using ESS.Domain.Bookings;
using ESS.Domain.Properties;
using ESS.Domain.Rentals;
using ESS.Domain.Tenants;

namespace ESS.Application.Bookings.Get
{
    internal static class BookingDtoFactory
    {
        public static BookingDto Create(Booking booking, RentalOption rentalOption, Property property, Tenant tenant)
        {
            var now = DateTime.UtcNow;
            var status = now < booking.StartDate
                ? BookingStatus.Upcoming
                : now > booking.EndDate
                    ? BookingStatus.Completed
                    : BookingStatus.Active;

            return new BookingDto(
                booking.Id,
                rentalOption.Id,
                rentalOption.Name ?? "",
                property.Id,
                property.Name ?? "",
                tenant.Id,
                tenant.Name ?? "",
                tenant.Email ?? "",
                tenant.Phone ?? "",
                booking.StartDate,
                booking.EndDate,
                status);
        }
    }
}
