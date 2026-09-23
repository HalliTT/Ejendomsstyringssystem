using AuthService.Domain.Common;
using MediatR;

namespace AuthService.Application.Organizations.List
{
    public sealed class Handler : IRequestHandler<Query, Result<IReadOnlyList<OrganizationResult>>>
    {
        private readonly IOrganizationRepository _organizations;

        public Handler(IOrganizationRepository organizations)
        {
            _organizations = organizations;
        }

        public async Task<Result<IReadOnlyList<OrganizationResult>>> Handle(Query request, CancellationToken ct)
        {
            var list = await _organizations.ListAsync(ct);
            return Result<IReadOnlyList<OrganizationResult>>.Success(list);
        }
    }
}
