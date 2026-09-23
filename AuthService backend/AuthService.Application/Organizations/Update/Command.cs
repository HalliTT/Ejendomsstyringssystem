using AuthService.Domain.Common;
using MediatR;

namespace AuthService.Application.Organizations.Update
{
    public sealed record Command
    (
        Guid OrganizationId,
        string Name,
        string? Description,
        string Slug,
        Guid? OwnerId
    ) : IRequest<Result<OrganizationResult>>;
}
