using ESS.Application.Bookings.Get;
using MediatR;

namespace ESS.Application.Bookings.List
{
    public sealed record Query : IRequest<IReadOnlyList<BookingDto>>;
}
