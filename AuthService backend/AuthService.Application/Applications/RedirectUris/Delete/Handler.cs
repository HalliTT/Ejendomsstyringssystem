using AuthService.Domain.Common;
using MediatR;

namespace AuthService.Application.Applications.RedirectUris.Delete
{
    public sealed class Handler : IRequestHandler<Command, Result>
    {
        private readonly IApplicationRepository _applications;

        public Handler(IApplicationRepository applications)
        {
            _applications = applications;
        }

        public async Task<Result> Handle(Command request, CancellationToken ct)
        {
            var deleted = await _applications.DeleteRedirectUriAsync(request.ApplicationId, request.RedirectUriId, ct);
            return deleted ? Result.Success() : Result.Failure();
        }
    }
}
