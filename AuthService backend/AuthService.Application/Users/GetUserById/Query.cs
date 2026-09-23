using AuthService.Contracts.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Users.GetUserById
{
    public sealed record Query : IRequest<List<UserDto>>;
}
