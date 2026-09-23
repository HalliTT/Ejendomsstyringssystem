using System.Net;

namespace AuthService.Domain.Audit
{
    public class AuthEvents
    {
        public Guid Id { get; set; }

        public AuthEventTypes EventType { get; set; }
        public Severity Severity { get; set; }

        public Guid? UserId { get; set; }
        public Guid? ClientId { get; set; }

        public string? Email {  get; set; }
        public IPAddress? IpAddress { get; set; }
        public string? UserAgent { get; set; }

        public bool Success { get; set; }
        public string? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public string? MetadataJson { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
