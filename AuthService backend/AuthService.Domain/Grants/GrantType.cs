using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Domain.Grants
{
    public enum GrantType
    {
        AuthorizationCode,
        RefreshToken,
        ClientCredentials
    }
}
