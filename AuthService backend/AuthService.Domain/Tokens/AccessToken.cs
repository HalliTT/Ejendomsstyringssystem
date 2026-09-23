using AuthService.Domain.Common;
using AuthService.Domain.Grants;
using AuthService.Domain.Sessions;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AuthService.Domain.Tokens
{
    public class AccessToken
    {
        private AccessToken() { }

        public Guid Id { get; set; }
        public Guid SessionId { get; private set; }

        public string Token { get; set; } = null!;
        public string? Scopes { get; set; }
        public bool IsRevoked { get; set; }

        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Session Session { get; private set; } = null!;

        public static (AccessToken token, string raw) Create(
            Guid sessionId,
            string? scopes,
            DateTime now)
        {
            var (raw, hash) = GenerateCode.GenerateSecureCode(32);
            return (
                new AccessToken
                {
                    Id = Guid.NewGuid(),
                    SessionId = sessionId,
                    Token = hash,
                    Scopes = scopes,
                    IsRevoked = false,
                    ExpiresAt = now.AddHours(1),
                    CreatedAt = now,
                    UpdatedAt = now
                }, 
                raw
            );
        }

        public void Revoke(DateTime now)
        {
            if (IsRevoked)
                return;

            IsRevoked = true;
            UpdatedAt = now;
        }

        public bool IsValid(DateTime now)
            => IsRevoked != true && ExpiresAt > now;
    }
}
