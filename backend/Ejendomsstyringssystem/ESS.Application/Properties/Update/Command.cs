using ESS.Application.Properties.Get;
using MediatR;

namespace ESS.Application.Properties.Update
{
    public sealed record Command
    (
        Guid PropertyId,
        string Name,
        string Address,
        string City,
        string Country,
        string Description
    ) : IRequest<PropertyDto?>;
}
