using AuthService.Application.Auth.Session.Tokens;
using AuthService.Application.Common.Interfaces;
using AuthService.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace AuthService.Api.Security
{
    public sealed class OpaqueBearerAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IAccessRepository _accessRepository;
        private readonly IClock _clock;

        public OpaqueBearerAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            IAccessRepository accessRepository,
            IClock clock)
            : base(options, logger, encoder)
        {
            _accessRepository = accessRepository;
            _clock = clock;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                return AuthenticateResult.NoResult();
            }

            var value = authorizationHeader.ToString();
            const string prefix = "Bearer ";
            if (!value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                return AuthenticateResult.NoResult();
            }

            var rawToken = value[prefix.Length..].Trim();
            if (string.IsNullOrWhiteSpace(rawToken))
            {
                return AuthenticateResult.Fail("Missing bearer token");
            }

            var accessToken = await _accessRepository.GetActiveWithSessionByRawTokenAsync(
                rawToken,
                _clock.UtcNow,
                Context.RequestAborted);

            if (accessToken is null || accessToken.Session is null || accessToken.Session.IsRevoked)
            {
                return AuthenticateResult.Fail("Invalid token");
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, accessToken.Session.UserId.ToString()),
                new("sub", accessToken.Session.UserId.ToString()),
                new("session_id", accessToken.SessionId.ToString())
            };

            var scopes = (accessToken.Scopes ?? string.Empty)
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (var scope in scopes)
            {
                claims.Add(new Claim("scope", scope));
            }

            var identity = new ClaimsIdentity(claims, OpaqueBearerAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, OpaqueBearerAuthenticationDefaults.AuthenticationScheme);

            return AuthenticateResult.Success(ticket);
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
