using AuthService.Domain.Common;
using MediatR;

namespace AuthService.Application.Applications.Update
{
    public sealed record Command
    (
        Guid OrganizationId,
        Guid ApplicationId,
        string Name,
        bool IsEnabled
    ) : IRequest<Result<ApplicationResult>>;
}
