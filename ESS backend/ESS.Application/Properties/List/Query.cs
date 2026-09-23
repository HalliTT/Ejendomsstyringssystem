using MediatR;

namespace ESS.Application.Properties.List
{
    public sealed record Query : IRequest<IReadOnlyList<PropertyListItemDto>>;
}
