using AuthService.Contracts.Users;
using AuthService.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Users.GetAllUsers
{
    public sealed record Query : IRequest<Result<List<UserDto>>>;
}
