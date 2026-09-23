using AuthService.Application.Organizations;
using AuthService.Domain.Common;
using MediatR;

namespace AuthService.Application.Applications.ListByOrganization
{
    public sealed class Handler : IRequestHandler<Query, Result<IReadOnlyList<ApplicationResult>>>
    {
        private readonly IApplicationRepository _applications;
        private readonly IOrganizationRepository _organizations;

        public Handler(IApplicationRepository applications, IOrganizationRepository organizations)
        {
            _applications = applications;
            _organizations = organizations;
        }

        public async Task<Result<IReadOnlyList<ApplicationResult>>> Handle(Query request, CancellationToken ct)
        {
            var orgExists = await _organizations.ExistsActiveAsync(request.OrganizationId, ct);
            if (!orgExists)
            {
                return Result<IReadOnlyList<ApplicationResult>>.Failure();
            }

            var list = await _applications.ListByOrganizationAsync(request.OrganizationId, ct);
            return Result<IReadOnlyList<ApplicationResult>>.Success(list);
        }
    }
}
