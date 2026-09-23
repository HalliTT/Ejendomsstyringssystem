using ESS.Application.Units.Get;
using ESS.Domain.Units;
using MediatR;

namespace ESS.Application.Units.Create
{
    public sealed record Command
    (
        string Name,
        string? Description,
        UnitStatus Status,
        Guid PropertyId
    ) : IRequest<UnitDto?>;
}
