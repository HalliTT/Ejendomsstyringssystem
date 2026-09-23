using AuthService.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Auth.Logout
{
    public record Command(Guid SessionId) : IRequest<Result>;
}
