using ESS.Domain.Rentals;

namespace ESS.Application.Rentals.ListMine
{
    public sealed record RentalOptionSummaryDto
    (
        Guid Id,
        string Name,
        decimal MonthlyRent,
        RentalOptionStatus Status,
        Guid PropertyId,
        string PropertyName
    );
}
