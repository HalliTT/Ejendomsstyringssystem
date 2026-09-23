using MediatR;

namespace ESS.Application.Bookings.Get
{
    public sealed record Query(Guid BookingId) : IRequest<BookingDto?>;
}
