using ESS.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESS.Application.Users
{
    public interface IAppUserRepository
    {
        Task<AppUser?> GetByIdAsync(Guid id, CancellationToken ct);
    }
}
