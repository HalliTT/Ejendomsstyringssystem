using AuthService.Domain.Applications;
using AuthService.Domain.Common;
using AuthService.Domain.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AuthService.Domain.Grants
{
    public sealed class Grant
    {
        private Grant() { }

        public Guid Id { get; set; }
        public string? CodeHash { get; set; } = null!;
        public GrantType GrantType { get; set; }
        public string? RedirectUri { get; set; }
        public string? Scopes { get; set; }
        public string CodeChallange { get; set; } = null!;
        public string CodeChallengeMethod { get; set; } = null!;
        public DateTime? RevokedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime? ConsumedAt { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public uint Version { get; set; }

        public Application Application { get; set; } = null!;
        public ICollection<AccessToken> AccessTokens { get; set; } = new List<AccessToken>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

        public bool IsRevoked => RevokedAt != null;
        public bool IsConsumed => ConsumedAt != null;

        public static Grant CreateAuthorizationCode(
            Guid applicationId,
            string redirectUri,
            string codeChallenge,
            string codeChallengeMethod,
            DateTime now)
        {
            var grant = new Grant
            {
                Id = Guid.NewGuid(),
                GrantType = GrantType.AuthorizationCode,
                ApplicationId = applicationId,
                RedirectUri = redirectUri,
                CreatedAt = now,
                UpdatedAt = now,
                ExpiresAt = now.AddMinutes(5),
                CodeChallange = codeChallenge,
                CodeChallengeMethod = codeChallengeMethod
            };

            return grant;
        }

        public void Authorize(Guid userId, DateTime now)
        {
            EnsureValid(now);

            var (raw, hash) = GenerateCode.GenerateSecureCode(32);

            UserId = userId;
            CodeHash = hash;
            ExpiresAt = now.AddMinutes(5);
            UpdatedAt = now;
        }

        public void Revoke(DateTime now)
        {
            if (RevokedAt != null)
            {
                return;
            }

            RevokedAt = now;
            UpdatedAt = now;
        }

        public void Approve(DateTime now)
        {
            EnsureValid(now);

            ExpiresAt = now.AddMinutes(10);
            UpdatedAt = now;
        }

        public void Consume(DateTime now)
        {
            if (ConsumedAt != null)
                throw new Exception("Grant already used");

            ConsumedAt = now;
            UpdatedAt = now;
        }

        public void EnsureValid(DateTime now)
        {
            if (RevokedAt != null)
                throw new Exception(GrantErrors.GrantRevoked);

            if(ExpiresAt < now)
                throw new Exception(GrantErrors.GrantExpired);
        }

        public void ConsumeToken(DateTime now)
        {
            EnsureValid(now);
            
            if (ConsumedAt != null)
                throw new Exception("Grant already used");

            //var access = AccessToken.Create(Id, Scopes, now);
            //var refresh = RefreshToken.Create(Id, now);

            //AccessTokens.Add(access);
            //RefreshTokens.Add(refresh);

            ConsumedAt = now;
            UpdatedAt = now;
        }

        //private static string GenerateSecureCode()
        //{
        //   return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        //}

        public void ValidatePkce(string verifier)
        {
            if (string.IsNullOrWhiteSpace(verifier))
                throw new InvalidOperationException("Missing PKCE verifier");

            var computed = PkceVerifier.Compute(verifier, CodeChallengeMethod);

            if (!string.Equals(computed, CodeChallange, StringComparison.Ordinal))
                throw new InvalidOperationException("Invalid PKCE verifier");
        }


        //public void ValidatePkce(string verifier)
        //{
        //    if (string.IsNullOrWhiteSpace(verifier))
        //        throw new InvalidOperationException("Missing PKCE verifier");

        //    var computed = PkceVerifier.Compute(verifier, CodeChallengeMethod);

        //    if (!string.Equals(computed, CodeChallange, StringComparison.Ordinal))
        //        throw new InvalidOperationException("Invalid PKCE verifier");
        //}

        public string ApproveConsent(DateTime now)
        {
            EnsureValid(now);

            if (UserId == Guid.Empty)
                throw new InvalidOperationException("Grant not authorized");

            ExpiresAt = now.AddMinutes(10);
            UpdatedAt = now;

            return $"code={Uri.EscapeDataString(CodeHash!)}";
        }

        public string DenyConsent(DateTime now)
        {
            Revoke(now);
            return "error=access_denied";
        }

        public void RevokeDueToTokenReuse(DateTime now)
        {
            RevokedAt = now;
            UpdatedAt = now;

            foreach (var refresh in RefreshTokens)
                refresh.Revoke(now);

            foreach (var access in AccessTokens)
                access.Revoke(now);
        }

    }
}
