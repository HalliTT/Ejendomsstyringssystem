using AuthService.Application.Audit;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

namespace AuthService.Infrastructure.Audit
{
    public class AuditHttpContext : IRequestContext
    {

        private readonly IHttpContextAccessor _accessor;

        public AuditHttpContext(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public IPAddress? IpAddress =>
            _accessor.HttpContext?.Connection.RemoteIpAddress;

        public string? UserAgent =>
            _accessor.HttpContext?.Request.Headers["User-Agent"].ToString() ?? "";

        public Guid? UserId
        {
            get
            {
                var value = _accessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return Guid.TryParse(value, out var id) ? id : null;
            }
        }
    }
}
