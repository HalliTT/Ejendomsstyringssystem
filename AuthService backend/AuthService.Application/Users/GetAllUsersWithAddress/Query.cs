using AuthService.Contracts.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Users.GetAllUsersWithAddress
{
    public sealed record Query : IRequest<List<UserWithAddressDto>>;
}
