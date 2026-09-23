using AuthService.Domain.Common;
using MediatR;

namespace AuthService.Application.Applications.Create
{
    public sealed record Command
    (
        Guid OrganizationId,
        string Name,
        IReadOnlyList<string> RedirectUris
    ) : IRequest<Result<ApplicationResult>>;
}
