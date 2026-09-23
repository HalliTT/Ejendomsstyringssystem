using AuthService.Contracts;
using AuthService.IntegrationTests.Auth;
using System.Net;
using System.Net.Http.Json;

namespace AuthService.IntegrationTests
{
    public sealed class AuthResponseEnvelopeTests : IClassFixture<ManagementApiFactory>
    {
        private readonly HttpClient _client;

        public AuthResponseEnvelopeTests(ManagementApiFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Logout_ReturnsSuccessEnvelope()
        {
            var response = await _client.PostAsJsonAsync("/auth/logout", new { sessionId = Guid.NewGuid() });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var payload = await response.Content.ReadFromJsonAsync<ApiResponse>();
            Assert.NotNull(payload);
            Assert.True(payload.Success);
            Assert.NotNull(payload.Message);
            Assert.Null(payload.Errors);
            Assert.NotEqual(default, payload.Timestamp);
        }

        [Fact]
        public async Task LogoutAll_ReturnsSuccessEnvelope()
        {
            var response = await _client.PostAsJsonAsync("/auth/logout-all", new { userId = Guid.NewGuid() });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var payload = await response.Content.ReadFromJsonAsync<ApiResponse>();
            Assert.NotNull(payload);
            Assert.True(payload.Success);
            Assert.NotNull(payload.Message);
            Assert.Null(payload.Errors);
            Assert.NotEqual(default, payload.Timestamp);
        }
    }
}