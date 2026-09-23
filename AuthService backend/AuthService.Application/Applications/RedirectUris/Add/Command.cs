using AuthService.Domain.Common;
using MediatR;

namespace AuthService.Application.Applications.RedirectUris.Add
{
    public sealed record Command(Guid ApplicationId, string RedirectUri) : IRequest<Result>;
}
