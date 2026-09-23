using AuthService.IntegrationTests.Auth;
using AuthService.Contracts;
using System.Net;
using System.Net.Http.Json;

namespace AuthService.IntegrationTests
{
    public sealed class ManagementAuthorizationTests : IClassFixture<ManagementApiFactory>
    {
        private readonly HttpClient _client;

        public ManagementAuthorizationTests(ManagementApiFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetOrganizations_WithoutToken_Returns401()
        {
            var response = await _client.GetAsync("/management/organizations");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

            var payload = await response.Content.ReadFromJsonAsync<ApiResponse>();
            Assert.NotNull(payload);
            Assert.False(payload.Success);
            Assert.Contains("invalid_client", payload.Errors ?? Array.Empty<string>());
        }

        [Fact]
        public async Task GetOrganizations_WithWrongScope_Returns403()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/management/organizations");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Test", "application.read");

            var response = await _client.SendAsync(request);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);

            var payload = await response.Content.ReadFromJsonAsync<ApiResponse>();
            Assert.NotNull(payload);
            Assert.False(payload.Success);
            Assert.Contains("unauthorized_client", payload.Errors ?? Array.Empty<string>());
        }

        [Fact]
        public async Task UpdateApplication_WithWrongScope_Returns403()
        {
            var organizationId = Guid.NewGuid();
            var applicationId = Guid.NewGuid();
            var request = new HttpRequestMessage(HttpMethod.Put, $"/management/organizations/{organizationId}/applications/{applicationId}")
            {
                Content = JsonContent.Create(new { name = "Updated Name", isEnabled = true })
            };
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Test", "organization.read");

            var response = await _client.SendAsync(request);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);

            var payload = await response.Content.ReadFromJsonAsync<ApiResponse>();
            Assert.NotNull(payload);
            Assert.False(payload.Success);
            Assert.Contains("unauthorized_client", payload.Errors ?? Array.Empty<string>());
        }
    }
}
