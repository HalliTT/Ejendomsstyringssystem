using AuthService.Domain.Common;
using MediatR;

namespace AuthService.Application.Organizations.List
{
    public sealed record Query : IRequest<Result<IReadOnlyList<OrganizationResult>>>;
}
