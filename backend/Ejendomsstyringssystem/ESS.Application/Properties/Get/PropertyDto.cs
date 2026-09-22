using ESS.Application.Rentals.Get;
using ESS.Application.Units.List;

namespace ESS.Application.Properties.Get
{
    public sealed record PropertyDto
    (
        Guid Id,
        string Name,
        string Address,
        string City,
        string Country,
        string Description,

        int OccupiedUnits,
        int TotalUnits,

        List<UnitListItemDto> Units,
        List<RentalOptionDto> RentalOptions
    );
}
