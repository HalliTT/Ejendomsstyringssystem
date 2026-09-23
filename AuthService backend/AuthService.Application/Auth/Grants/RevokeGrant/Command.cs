using AuthService.Application.Auth.Grants.ConsentApplication;
using AuthService.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Auth.Grants.RevokeGrant
{
    public sealed record Command(Guid GrantId, bool Approved) : IRequest<Result<ConsentResult>>;

}
