using AuthService.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Auth.Logout.AllDevices
{
    public record Command(Guid UserId) : IRequest<Result>;
}
