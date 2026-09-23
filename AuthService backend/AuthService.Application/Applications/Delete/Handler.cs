using AuthService.Application.Common.Interfaces;
using AuthService.Domain.Common;
using MediatR;

namespace AuthService.Application.Applications.Delete
{
    public sealed class Handler : IRequestHandler<Command, Result>
    {
        private readonly IApplicationRepository _applications;
        private readonly IClock _clock;

        public Handler(IApplicationRepository applications, IClock clock)
        {
            _applications = applications;
            _clock = clock;
        }

        public async Task<Result> Handle(Command request, CancellationToken ct)
        {
            if (request.OrganizationId == Guid.Empty || request.ApplicationId == Guid.Empty || request.DeletedBy == Guid.Empty)
            {
                return Result.Failure();
            }

            var deleted = await _applications.SoftDeleteAsync(request.OrganizationId, request.ApplicationId, request.DeletedBy, _clock.UtcNow, ct);
            return deleted ? Result.Success() : Result.Failure();
        }
    }
}
