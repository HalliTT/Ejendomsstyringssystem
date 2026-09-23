using AuthService.Application.Common.Interfaces;
using AuthService.Domain.Common;
using MediatR;

namespace AuthService.Application.Applications.Update
{
    public sealed class Handler : IRequestHandler<Command, Result<ApplicationResult>>
    {
        private readonly IApplicationRepository _applications;
        private readonly IClock _clock;

        public Handler(IApplicationRepository applications, IClock clock)
        {
            _applications = applications;
            _clock = clock;
        }

        public async Task<Result<ApplicationResult>> Handle(Command request, CancellationToken ct)
        {
            if (request.OrganizationId == Guid.Empty || request.ApplicationId == Guid.Empty || string.IsNullOrWhiteSpace(request.Name))
            {
                return Result<ApplicationResult>.Failure();
            }

            var updated = await _applications.UpdateAsync(
                request.OrganizationId,
                request.ApplicationId,
                request.Name.Trim(),
                request.IsEnabled,
                _clock.UtcNow,
                ct);

            return updated is null
                ? Result<ApplicationResult>.Failure()
                : Result<ApplicationResult>.Success(updated);
        }
    }
}
