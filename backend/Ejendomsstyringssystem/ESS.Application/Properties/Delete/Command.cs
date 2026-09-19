using MediatR;

namespace ESS.Application.Properties.Delete
{
    public sealed record Command(Guid PropertyId) : IRequest<bool>;
}
