using AuthService.Domain.Common;
using MediatR;

namespace AuthService.Application.Applications.ListByOrganization
{
    public sealed record Query(Guid OrganizationId) : IRequest<Result<IReadOnlyList<ApplicationResult>>>;
}
