using AuthService.Domain.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Auth.Session
{
    public interface IFingerprintBuilder
    {
        TokenFingerprint Build(string? deviceId);
    }
}
