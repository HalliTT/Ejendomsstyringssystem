using AuthService.Application.Common.Interfaces;
using AuthService.Domain.Common;
using MediatR;

namespace AuthService.Application.Organizations.Update
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

            if (request.OrganizationId == Guid.Empty || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(slug))
            {
                return Result<OrganizationResult>.Failure();
            }

            var ownerId = request.OwnerId.HasValue && request.OwnerId.Value != Guid.Empty
                ? request.OwnerId
                : null;

            if (await _organizations.SlugExistsForOtherAsync(request.OrganizationId, slug, ct))
            {
                return Result<OrganizationResult>.Failure();
            }

            var updated = await _organizations.UpdateAsync(
                request.OrganizationId,
                name,
                request.Description?.Trim(),
                slug,
                ownerId,
                _clock.UtcNow,
                ct);

            return updated is null
                ? Result<OrganizationResult>.Failure()
                : Result<OrganizationResult>.Success(updated);
        }
    }
}
