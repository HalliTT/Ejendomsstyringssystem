using MediatR;

namespace ESS.Application.Bookings.Update
{
    public sealed record Command
    (
        Guid BookingId,
        Guid RentalOptionId,
        Guid TenantId,
        DateTime StartDate,
        DateTime EndDate
    ) : IRequest<BookingMutationResult>;
}
