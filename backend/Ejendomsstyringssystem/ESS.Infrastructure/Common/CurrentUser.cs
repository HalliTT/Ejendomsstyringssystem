using ESS.Application.Common.Interface;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ESS.Infrastructure.Common
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _accessor;
        public CurrentUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
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
