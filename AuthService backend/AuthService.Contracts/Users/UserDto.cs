using System;
using System.Collections.Generic;
using System.Text;
using AuthService.Contracts.Addresses;

namespace AuthService.Contracts.Users
{
    public class UserDto
    {
        public Guid Id { get; init; }
        public string Email { get; init; } = default!;
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? DisplayName { get; init; }
        public bool IsActive { get; init; }
        public bool IsVerified { get; init; }
        public DateTime CreatedAt { get; init; }

        public AddressDto? Address { get; init; }
    }
}
