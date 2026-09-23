using AuthService.Domain.Common;
using AuthService.Domain.Grants;
using AuthService.Domain.Sessions;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AuthService.Domain.Tokens
{
    public class RefreshToken
    {
        private RefreshToken() { }

        public Guid Id { get; set; }
        public Guid SessionId { get; set; }

        public string TokenHash { get; private set; } = null!;

        public Guid? ReplacedByTokenId { get; set; }

        public DateTime? RevokedAt { get; set; }
        public DateTime ExpiresAt { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public uint Version { get; set; }

        public Session Session { get; private set; } = null!;
        public RefreshToken? ReplacedByToken { get; set; }

        public static (RefreshToken token, string raw) Create(
            Guid sessionId,
            DateTime now)
        {
            var (raw, hash) = GenerateCode.GenerateSecureCode(48);

            return (
                new RefreshToken
                {
                    Id = Guid.NewGuid(),
                    SessionId = sessionId,
                    TokenHash = hash,
                    ExpiresAt = now.AddDays(30),
                    CreatedAt = now,
                    UpdatedAt = now
                },
                raw
            );
        }

        public void Revoke(DateTime now)
        {
            if (RevokedAt != null)
                return;

            RevokedAt = now;
            UpdatedAt = now;
        }

        public bool IsValid(DateTime now)
            => RevokedAt == null && ExpiresAt > now;
    }
}
