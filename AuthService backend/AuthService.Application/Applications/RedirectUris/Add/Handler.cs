using AuthService.Application.Auth.Verify.ClientVerification;
using AuthService.Application.Common.Interfaces;
using AuthService.Domain.Common;
using MediatR;

namespace AuthService.Application.Applications.RedirectUris.Add
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
            if (string.IsNullOrWhiteSpace(request.RedirectUri))
            {
                return Result.Failure();
            }

            var normalized = RedirectUriHelper.Normalize(request.RedirectUri);
            var added = await _applications.AddRedirectUriAsync(request.ApplicationId, normalized, _clock.UtcNow, ct);

            return added ? Result.Success() : Result.Failure();
        }
    }
}
