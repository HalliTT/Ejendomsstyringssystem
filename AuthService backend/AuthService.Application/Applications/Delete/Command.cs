using AuthService.Domain.Common;
using MediatR;

namespace AuthService.Application.Applications.Delete
{
    public sealed record Command(Guid OrganizationId, Guid ApplicationId, Guid DeletedBy) : IRequest<Result>;
}
