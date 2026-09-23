using System.ComponentModel.DataAnnotations;

namespace ESS.Api.Contracts.Bookings
{
    public sealed record CreateBookingRequest
    (
        [Required] Guid RentalOptionId,
        [Required] Guid TenantId,
        [Required] DateTime StartDate,
        [Required] DateTime EndDate
    );
}
