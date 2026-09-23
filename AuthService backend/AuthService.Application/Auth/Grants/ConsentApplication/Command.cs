using AuthService.Application.Audit;
using AuthService.Domain.Audit;
using AuthService.Domain.Common;
using MediatR;

namespace AuthService.Application.Auth.Grants.ConsentApplication
{
    public sealed record Command(Guid GrantId, bool Approved)
        : IRequest<Result<ConsentResult>>;
}
