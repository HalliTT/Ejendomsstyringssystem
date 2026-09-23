using AuthService.Application.Common.Interfaces;
using AuthService.Domain.Common;
using MediatR;

namespace AuthService.Application.Organizations.Delete
{
    public sealed class Handler : IRequestHandler<Command, Result>
    {
        private readonly IOrganizationRepository _organizations;
        private readonly IClock _clock;

        public Handler(IOrganizationRepository organizations, IClock clock)
        {
            _organizations = organizations;
            _clock = clock;
        }

        public async Task<Result> Handle(Command request, CancellationToken ct)
        {
            if (request.OrganizationId == Guid.Empty || request.DeletedBy == Guid.Empty)
            {
                return Result.Failure();
            }

            var deleted = await _organizations.SoftDeleteAsync(request.OrganizationId, request.DeletedBy, _clock.UtcNow, ct);
            return deleted ? Result.Success() : Result.Failure();
        }
    }
}
