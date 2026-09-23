using ESS.Application.Tenants.Get;
using MediatR;

namespace ESS.Application.Tenants.List
{
    public sealed class Handler : IRequestHandler<Query, IReadOnlyList<TenantDto>>
    {
        private readonly ITenantRepository _tenantRepository;

        public Handler(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public async Task<IReadOnlyList<TenantDto>> Handle(Query request, CancellationToken ct)
        {
            var tenants = await _tenantRepository.ListAsync(ct);

            return tenants
                .Select(t => new TenantDto(t.Id, t.Name ?? "", t.Email ?? "", t.Phone ?? ""))
                .ToList();
        }
    }
}
