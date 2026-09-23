using ESS.Application.Tenants.Get;
using MediatR;

namespace ESS.Application.Tenants.List
{
    public sealed record Query : IRequest<IReadOnlyList<TenantDto>>;
}
