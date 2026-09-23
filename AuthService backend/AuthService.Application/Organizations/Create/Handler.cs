using AuthService.Application.Common.Interfaces;
using AuthService.Domain.Common;
using MediatR;

namespace AuthService.Application.Organizations.Create
{
    public sealed class Handler : IRequestHandler<Command, Result<OrganizationResult>>
    {
        private readonly IOrganizationRepository _organizations;
        private readonly IClock _clock;

        public Handler(IOrganizationRepository organizations, IClock clock)
        {
            _organizations = organizations;
            _clock = clock;
        }

        public async Task<Result<OrganizationResult>> Handle(Command request, CancellationToken ct)
        {
            var name = request.Name?.Trim();
            var slug = request.Slug?.Trim().ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(slug) || request.OwnerId == Guid.Empty)
            {
                return Result<OrganizationResult>.Failure();
            }

            if (await _organizations.SlugExistsAsync(slug, ct))
            {
                return Result<OrganizationResult>.Failure();
            }

            var created = await _organizations.CreateAsync(
                name,
                request.Description?.Trim(),
                slug,
                request.OwnerId,
                request.OwnerId,
                _clock.UtcNow,
                ct);

            return Result<OrganizationResult>.Success(created);
        }
    }
}
