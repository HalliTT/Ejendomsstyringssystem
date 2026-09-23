using AuthService.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Auth.Verify.ClientVerification
{
    public sealed record Query(string ClientId, string RedirectUri) : IRequest<Result>;
}
