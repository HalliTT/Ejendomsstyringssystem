using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Auth.Login
{
    public sealed record ApplicationInfo(string Name, IReadOnlyList<string> Scopes, Guid Id);

    public sealed record LoginResult
    (
        string AuthorizationCode,
        bool IsUserInOrganization,
        ApplicationInfo Application,
        IReadOnlyList<string> RequestedScopes
    );
}
