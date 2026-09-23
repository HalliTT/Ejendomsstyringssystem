using System.ComponentModel.DataAnnotations;

namespace ESS.Api.Contracts.Bookings
{
    public sealed record UpdateBookingRequest
    (
        [Required] Guid RentalOptionId,
        [Required] Guid TenantId,
        [Required] DateTime StartDate,
        [Required] DateTime EndDate
    );
}
