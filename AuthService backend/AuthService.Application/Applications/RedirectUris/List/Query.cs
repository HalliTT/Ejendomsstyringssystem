using AuthService.Domain.Common;
using MediatR;

namespace AuthService.Application.Applications.RedirectUris.List
{
    public sealed record Query(Guid ApplicationId) : IRequest<Result<IReadOnlyList<ApplicationRedirectUriResult>>>;
}
