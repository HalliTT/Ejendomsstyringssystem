using MediatR;

namespace ESS.Application.Units.Delete
{
    public sealed record Command(Guid UnitId) : IRequest<bool>;
}
