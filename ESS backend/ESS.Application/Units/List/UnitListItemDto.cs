using ESS.Domain.Units;

namespace ESS.Application.Units.List
{
    public sealed record UnitListItemDto
    (
        Guid Id,
        string Name,
        string Description,
        UnitStatus Status
    );
}
