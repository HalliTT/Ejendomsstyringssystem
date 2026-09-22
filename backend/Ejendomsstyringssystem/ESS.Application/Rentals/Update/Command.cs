using ESS.Application.Rentals.Get;
using ESS.Domain.Rentals;
using MediatR;

namespace ESS.Application.Rentals.Update
{
    public sealed record Command
    (
        Guid RentalOptionId,
        string Name,
        decimal MonthlyRent,
        RentalOptionStatus Status,
        List<Guid> UnitIds
    ) : IRequest<RentalOptionDto?>;
}
