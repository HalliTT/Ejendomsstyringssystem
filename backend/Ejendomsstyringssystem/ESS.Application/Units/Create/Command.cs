using ESS.Application.Units.Get;
using MediatR;

namespace ESS.Application.Units.Create
{
    public sealed record Command
    (
        string Name,
        string? Description,
        string Status,
        Guid PropertyId
    ) : IRequest<UnitDto>;
}
