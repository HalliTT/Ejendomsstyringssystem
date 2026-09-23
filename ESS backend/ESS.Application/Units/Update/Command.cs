using ESS.Application.Units.Get;
using ESS.Domain.Units;
using MediatR;

namespace ESS.Application.Units.Update
{
    public sealed record Command
    (
        Guid UnitId,
        string Name,
        string Description,
        UnitStatus Status
    ) : IRequest<UnitDto?>;
}
