using AuthService.Domain.Tokens;

namespace AuthService.Domain.Sessions
{
    public class Session
    {
        private Session() { }

        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public DateTime LastActivity { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public bool IsRevoked { get; private set; }

        public uint Version { get; set; }



        public ICollection<TokenFingerprint> Fingerprints { get; private set; } = new List<TokenFingerprint>();

        private readonly List<RefreshToken> _refreshTokens = new();
        private readonly List<AccessToken> _accessTokens = new();

        public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens;
        public IReadOnlyCollection<AccessToken> AccessTokens => _accessTokens;

        public static Session Create(
           Guid userId,
           TokenFingerprint fingerprint,
           DateTime now)
        {
            return new Session
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CreatedAt = now,
                UpdatedAt = now,
                LastActivity = now,
                Fingerprints = new List<TokenFingerprint> { fingerprint }
            };
        }

        public (string accessRaw, string refreshRaw, AccessToken access, RefreshToken refresh) IssueTokens(DateTime now, string? scopes)
        {
            if (IsRevoked)
                throw new Exception("Session revoked");

            var (refresh, refreshRaw) = RefreshToken.Create(Id, now);
            var (access, accessRaw) = AccessToken.Create(Id, scopes, now);

            _refreshTokens.Add(refresh);
            _accessTokens.Add(access);

            LastActivity = now;
            UpdatedAt = now;

            return (accessRaw, refreshRaw, access, refresh);
        }

        public (string accessRaw, string refreshRaw, AccessToken access, RefreshToken refresh) RotateRefreshToken(RefreshToken current, DateTime now, string? scopes)
        {
            if (IsRevoked)
                throw new Exception("Session revoked");

            if (!current.IsValid(now))
                throw new Exception("Refresh token invalid");

            if (current.ReplacedByTokenId != null)
            {
                // TOKEN REUSE DETECTED → FULL SESSION COMPROMISE
                Revoke(now);
                throw new Exception("Refresh token reuse detected");
            }

            var (newRefresh, refreshRaw) = RefreshToken.Create(Id, now);
            var (newAccess, accessRaw) = AccessToken.Create(Id, scopes, now);

            current.ReplacedByTokenId = newRefresh.Id;
            current.RevokedAt = now;
            current.UpdatedAt = now;

            _refreshTokens.Add(newRefresh);
            _accessTokens.Add(newAccess);

            LastActivity = now;
            UpdatedAt = now;

            return (accessRaw, refreshRaw, newAccess, newRefresh);
        }

        public void Revoke(DateTime now)
        {
            if (IsRevoked) return;

            IsRevoked = true;
            UpdatedAt = now;

            foreach (var r in _refreshTokens)
                r.Revoke(now);

            foreach (var a in _accessTokens)
                a.Revoke(now);
        }
    }
}
