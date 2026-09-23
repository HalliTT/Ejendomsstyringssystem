using AuthService.Application.Users;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AuthService.Api.Security
{
    public sealed class ManagementAccessRequirementHandler : AuthorizationHandler<ManagementAccessRequirement>
    {
        private readonly IUserRepository _userRepository;

        public ManagementAccessRequirementHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ManagementAccessRequirement requirement)
        {
            var value = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? context.User.FindFirst("sub")?.Value;

            if (!Guid.TryParse(value, out var userId))
            {
                return;
            }

            var user = await _userRepository.GetByIdAsync(userId, CancellationToken.None);
            if (user is null || !user.IsAdmin)
            {
                return;
            }

            context.Succeed(requirement);
        }
    }
}
