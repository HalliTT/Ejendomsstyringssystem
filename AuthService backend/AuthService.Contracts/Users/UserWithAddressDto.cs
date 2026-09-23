using AuthService.Contracts.Addresses;

namespace AuthService.Contracts.Users
{
    public class UserWithAddressDto
    {
        public Guid Id { get; init; }
        public string Email { get; init; } = default!;
        public AddressDto? Address { get; init; }
    }
}
