using MediatR;

namespace ESS.Application.Bookings.Create
{
    public sealed record Command
    (
        Guid RentalOptionId,
        Guid TenantId,
        DateTime StartDate,
        DateTime EndDate
    ) : IRequest<BookingMutationResult>;
}
