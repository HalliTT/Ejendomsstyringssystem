using AuthService.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace AuthService.IntegrationTests.Auth
{
    internal sealed class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string SchemeName = "Test";

        public TestAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var headerValue = authorizationHeader.ToString();
            if (!headerValue.StartsWith(SchemeName + " ", StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var rawScopes = headerValue[SchemeName.Length..].Trim();
            var scopes = rawScopes
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, "11111111-1111-1111-1111-111111111111"),
                new("sub", "11111111-1111-1111-1111-111111111111")
            };

            foreach (var scope in scopes)
            {
                claims.Add(new Claim("scope", scope));
            }

            var identity = new ClaimsIdentity(claims, SchemeName);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, SchemeName);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 401;
            Response.ContentType = "application/json";

            return Response.WriteAsJsonAsync(ApiResponse.ErrorResponse(
                "invalid_client",
                "Unauthorized"));
        }

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 403;
            Response.ContentType = "application/json";

            return Response.WriteAsJsonAsync(ApiResponse.ErrorResponse(
                "unauthorized_client",
                "Forbidden"));
        }
    }
}
