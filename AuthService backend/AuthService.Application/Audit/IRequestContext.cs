using System.Net;

namespace AuthService.Application.Audit
{
    public interface IRequestContext
    {
        IPAddress? IpAddress { get; }
        string? UserAgent { get; }
        Guid? UserId { get; }
    }
}
