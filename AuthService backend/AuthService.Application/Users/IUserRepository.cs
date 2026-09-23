using System;
using System.Collections.Generic;
using System.Text;
using AuthService.Domain.Users;


namespace AuthService.Application.Users
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<User?> GetByEmailAsync(string email, CancellationToken ct);
        Task<IReadOnlyList<User>> GetUserAsync(CancellationToken ct);
        Task<IReadOnlyList<User>> GetAllAsync(CancellationToken ct);
        Task<IReadOnlyList<User>> GetAllWithAddressAsync(CancellationToken ct);
    }
}
