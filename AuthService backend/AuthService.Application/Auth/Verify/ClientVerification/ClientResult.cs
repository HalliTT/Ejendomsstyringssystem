using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Auth.Verify.ClientVerification
{
    public sealed record ClientResult(
        Guid Id,
        Guid ClientId,
        bool IsEnabled,
        DateTime? SoftDeletedAt,
        IReadOnlyList<string> RedirectUris
    );
}
