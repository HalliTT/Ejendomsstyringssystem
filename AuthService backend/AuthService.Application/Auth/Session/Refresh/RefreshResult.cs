using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Auth.Session.Refresh
{
    public sealed record RefreshResult
    (
        string AccessToken,
        DateTime AccessTokenExpires,
        string RefreshToken,
        DateTime RefreshTokenExpires,
        Guid SessionId,
        Guid UserId
    );
}
