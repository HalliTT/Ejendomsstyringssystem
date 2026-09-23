using AuthService.Domain.Common;
using MediatR;

namespace AuthService.Application.Organizations.Delete
{
    public sealed record Command(Guid OrganizationId, Guid DeletedBy) : IRequest<Result>;
}
