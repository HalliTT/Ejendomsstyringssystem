namespace AuthService.Contracts.Management
{
    public sealed record CreateUserRequest
    (
        string Email,
        string? FirstName,
        string LastName,
        string Password,
        Guid CreatedBy,
        Guid AddressId,
        bool IsAdmin
    );
}
