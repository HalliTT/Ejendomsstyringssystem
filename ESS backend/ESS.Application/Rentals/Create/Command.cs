using ESS.Application.Rentals.Get;
using ESS.Domain.Rentals;
using MediatR;

namespace ESS.Application.Rentals.Create
{
    public sealed record Command
    (
        string Name,
        decimal MonthlyRent,
        RentalOptionStatus Status,
        Guid PropertyId,
        List<Guid> UnitIds
    ) : IRequest<RentalOptionDto?>;
}
