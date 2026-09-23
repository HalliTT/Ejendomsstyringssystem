using AuthService.Domain.Common;
using MediatR;

namespace AuthService.Application.Applications.RedirectUris.Delete
{
    public sealed record Command(Guid ApplicationId, Guid RedirectUriId) : IRequest<Result>;
}
