using MediatR;

namespace ESS.Application.Properties.Get
{
    public sealed record Query(Guid PropertyId) : IRequest<PropertyDto?>;
}
