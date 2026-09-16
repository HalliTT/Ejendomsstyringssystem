using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace ESS.Api.Security
{
    public sealed class EssBearerAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EssBearerAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, IHttpClientFactory httpClientFactory) : base(options, logger, encoder) => _httpClientFactory = httpClientFactory;
        
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("Authorization", out var header))
                return AuthenticateResult.NoResult();

            var raw = header.ToString();
            if (!raw.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                return AuthenticateResult.NoResult();

            var client = _httpClientFactory.CreateClient("AuthService");
            var request = new HttpRequestMessage(HttpMethod.Get, "auth/userinfo");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", raw["Bearer".Length..].Trim());

            var response = await client.SendAsync(request, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
                return AuthenticateResult.Fail("Invalid token");

            var payload = await response.Content.ReadFromJsonAsync<UserInfoResponse>(
                new JsonSerializerOptions(JsonSerializerDefaults.Web),
                Context.RequestAborted);
            if (payload is not { Success: true, Data: not null })
                return AuthenticateResult.Fail("Invalid token");

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, payload.Data.UserId.ToString()),
                new(ClaimTypes.Email, payload.Data.Email ?? "")
            };

            var identity = new ClaimsIdentity(claims, "EssBearer");
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), "EssBearer");
            return AuthenticateResult.Success(ticket);
        }
    }
}
