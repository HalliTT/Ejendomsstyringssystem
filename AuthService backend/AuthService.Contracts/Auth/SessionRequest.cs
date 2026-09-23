using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Contracts.Auth
{
    public sealed record SessionRequest
    (
        //Guid ClientId,
        string code,
        string codeVerifier,
        string? deviceId
        //string GrantType,
        //string RedirectUri,
        //string? refreshToken,

        //string? userAgent,
        //string? platform,
        //string? screenResolution
        //Guid clientId,
        //string code,
        //string codeVerifier,
        //string grantType,
        //string redirectUri,
        //string DeviceFingerprint,
        //string IpHash,
        //string UserAgentHash
    );
}
