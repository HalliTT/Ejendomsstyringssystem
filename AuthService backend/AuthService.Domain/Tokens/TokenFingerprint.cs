using AuthService.Domain.Sessions;

namespace AuthService.Domain.Tokens
{
    public class TokenFingerprint
    {
        public Guid Id { get; set; }
        public Guid SessionId { get; private set; }
        public string DeviceFingerprint { get; init; }
        public string IpRangeHash { get; init; }
        public string UserAgentHash { get; init; }
        public Session Session { get; private set; } = null!;
    }
}
