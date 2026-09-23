using ESS.Application.Users;
using ESS.Domain.Owners;
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
                var owner = CreatOwnerFor(email, displayName);
                _context.Owners.Add(owner);

                user = new Domain.Users.AppUser
                {
                    Id = userId,
                    Email = email ?? "",
                    DisplayName = displayName,
                    Owner = owner,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsEnabled = true,
                };
                _context.AppUsers.Add(user);
            }
            else
            {
                if (user.Email != email || user.DisplayName != displayName)
                {
                    user.Email = email ?? user.Email;
                    user.DisplayName = displayName ?? user.DisplayName;
                    user.UpdatedAt = DateTime.UtcNow;
                }

                if (user.OwnerId is null)
                {
                    var owner = CreatOwnerFor(email, displayName);
                    _context.Owners.Add(owner);
                    user.Owner = owner;
                    user.UpdatedAt = DateTime.UtcNow;
                }
            }

            await _context.SaveChangesAsync(ct);
        }

        private static Owner CreatOwnerFor(string? email, string? displayName) => new()
        {
            Id = Guid.NewGuid(),
            Name = displayName ?? email ?? "Owner",
            DisplayName = displayName,
            Email = email ?? "",
            IsEnabled = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
    }
}
