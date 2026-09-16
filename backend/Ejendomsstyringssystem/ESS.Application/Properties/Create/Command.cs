using ESS.Application.Properties.Get;
using MediatR;

namespace ESS.Application.Properties.Create
{
    public sealed record Command
    (
        string Name,
        string Address,
        string City,
        string Country,
        string Description
    ) : IRequest<PropertyDto>;
}
