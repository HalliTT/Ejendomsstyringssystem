using ESS.Domain.Units;

namespace ESS.Application.Units.Get
{
    public sealed record UnitDto
    (
        Guid Id,
        Guid PropertyId,
        string Name,
        string Description,
        UnitStatus Status
    );
}