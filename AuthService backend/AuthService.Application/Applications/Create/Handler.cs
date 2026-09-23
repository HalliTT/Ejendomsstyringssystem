using AuthService.Application.Auth.Verify.ClientVerification;
using AuthService.Application.Common.Interfaces;
using AuthService.Application.Organizations;
using AuthService.Domain.Common;
using MediatR;

namespace AuthService.Application.Applications.Create
{
    public sealed class Handler : IRequestHandler<Command, Result<ApplicationResult>>
    {
        private readonly IApplicationRepository _applications;
        private readonly IOrganizationRepository _organizations;
        private readonly IClock _clock;

        public Handler(IApplicationRepository applications, IOrganizationRepository organizations, IClock clock)
        {
            _applications = applications;
            _organizations = organizations;
            _clock = clock;
        }

        public async Task<Result<ApplicationResult>> Handle(Command request, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return Result<ApplicationResult>.Failure();
            }

            var orgExists = await _organizations.ExistsActiveAsync(request.OrganizationId, ct);
            if (!orgExists)
            {
                return Result<ApplicationResult>.Failure();
            }

            var normalizedRedirectUris = (request.RedirectUris ?? Array.Empty<string>())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => RedirectUriHelper.Normalize(x))
                .Distinct(StringComparer.Ordinal)
                .ToList();

            if (normalizedRedirectUris.Count == 0)
            {
                return Result<ApplicationResult>.Failure();
            }

            var created = await _applications.CreateAsync(
                request.OrganizationId,
                request.Name.Trim(),
                normalizedRedirectUris,
                _clock.UtcNow,
                ct);

            return Result<ApplicationResult>.Success(created);
        }
    }
}
