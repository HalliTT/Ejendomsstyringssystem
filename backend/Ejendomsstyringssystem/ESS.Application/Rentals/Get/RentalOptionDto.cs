using ESS.Domain.Rentals;

namespace ESS.Application.Rentals.Get
{
    public sealed record RentalOptionDto
    (
        Guid Id,
        Guid PropertyId,
        string Name,
        decimal MonthlyRent,
        RentalOptionStatus Status,
        List<Guid> UnitIds
    );
}
