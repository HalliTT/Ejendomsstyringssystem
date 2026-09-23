using AuthService.Application.Auth.Session;
using AuthService.Application.Common.Behaviors;
using AuthService.Domain.Common;
using AuthService.Domain.Tokens;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.WebRequestMethods;

namespace AuthService.Infrastructure.Session
{
    public sealed class FingerprintBuilder : IFingerprintBuilder
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FingerprintBuilder(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public TokenFingerprint Build(string? deviceId)
        {
            var ctx = _httpContextAccessor.HttpContext!;

            var ip = ctx.Connection.RemoteIpAddress?.ToString() ?? "";
            var ua = ctx.Request.Headers["User-Agent"].ToString();

            var ipRange = IpHelper.GetIpRange(ip);

            return new TokenFingerprint
            {
                Id = Guid.NewGuid(),
                DeviceFingerprint = SecurityHash.Sha256(deviceId ?? ""),
                IpRangeHash = SecurityHash.Sha256(ipRange),
                UserAgentHash = SecurityHash.Sha256(ua)
            };
        }
    }
}
