using ESS.Application.Users;
using ESS.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESS.Infrastructure.Users
{
    public class UserProvisioningService : IUserProvisioningService
    {
        private readonly EssDbContext _context;
        public UserProvisioningService(EssDbContext context)
        {
            _context = context;
        }

        public async Task EnsureUserAsync(Guid userId, string? email, string? displayName, CancellationToken ct)
        {
            var user = await _context.AppUsers.FindAsync(new object[] { userId }, ct);
            if (user is null)
            {
                _context.AppUsers.Add(new Domain.Users.AppUser
                {
                    Id = userId,
                    Email = email ?? "",
                    DisplayName = displayName,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsEnabled = true,
                });
            }
            else if (user.Email != email || user.DisplayName != displayName)
            {
                user.Email = email ?? user.Email;
                user.DisplayName = displayName ?? user.DisplayName;
                user.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }
    }
}
