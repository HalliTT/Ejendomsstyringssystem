using AuthService.Domain.Common;
using MediatR;

namespace AuthService.Application.Applications.RedirectUris.List
{
    public sealed class Handler : IRequestHandler<Query, Result<IReadOnlyList<ApplicationRedirectUriResult>>>
    {
        private readonly IApplicationRepository _applications;

        public Handler(IApplicationRepository applications)
        {
            _applications = applications;
        }

        public async Task<Result<IReadOnlyList<ApplicationRedirectUriResult>>> Handle(Query request, CancellationToken ct)
        {
            var list = await _applications.ListRedirectUrisAsync(request.ApplicationId, ct);
            return Result<IReadOnlyList<ApplicationRedirectUriResult>>.Success(list);
        }
    }
}
