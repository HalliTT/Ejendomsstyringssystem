using AuthService.Domain.Common;
using MediatR;

namespace AuthService.Application.Organizations.Create
{
    public sealed record Command
    (
        string Name,
        string? Description,
        string Slug,
        Guid OwnerId
    ) : IRequest<Result<OrganizationResult>>;
}
