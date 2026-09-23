using System;
using System.Collections.Generic;
using System.Text;
using AuthService.Application.Common;

namespace AuthService.Application.Applications
{
    public sealed record ApplicationResult
    (
        Guid Id,
        Guid ClientId,
        string? Name,
        bool IsEnabled,
        Guid OrganizationId,
        DateTime? SoftDeletedAt,
        IReadOnlyList<string> RedirectUris,
        string? ClientSecret = null,
        UserMinimalResult? DeletedByUser = null
    );
}
