using MediatR;

namespace ESS.Application.Rentals.Delete
{
    public sealed record Command(Guid RentalOptionId) : IRequest<bool>;
}
