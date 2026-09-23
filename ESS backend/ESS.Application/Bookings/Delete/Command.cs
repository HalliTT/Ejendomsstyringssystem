using MediatR;

namespace ESS.Application.Bookings.Delete
{
    public sealed record Command(Guid BookingId) : IRequest<bool>;
}
