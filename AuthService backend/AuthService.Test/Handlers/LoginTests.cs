using AuthService.Application.Applications;
using AuthService.Application.Audit;
using AuthService.Application.Auth.Grants;
using AuthService.Application.Auth.Login;
using AuthService.Application.Auth.Password;
using AuthService.Application.Auth.Verify;
using AuthService.Application.Common.Interfaces;
using AuthService.Application.Organizations;
using AuthService.Application.Users;
using AuthService.Domain.Audit;
using AuthService.Domain.Grants;
using AuthService.Domain.Users;
using Moq;

namespace AuthService.Test.Handlers
{
    public class LoginTests
    {
        private readonly Mock<IUserRepository> _users = new();
        private readonly Mock<IApplicationRepository> _applications = new();
        private readonly Mock<IGrantRepository> _grants = new();
        private readonly Mock<IPasswordHasher> _hasher = new();
        private readonly Mock<IClock> _clock = new();
        private readonly Mock<IAuthorizationRequestStore> _authStore = new();
        private readonly Mock<IOrganizationRepository> _org = new();
        private readonly Mock<IAuditService> _audit = new();

        private Handler CreateHandler()
        {
            return new Handler(_users.Object, _applications.Object, _grants.Object, _hasher.Object, _clock.Object, _authStore.Object, _org.Object, _audit.Object);
        }

        [Fact]
        public async Task Handle_CorrectCredentialsAndValidApp_AuthorizesGrantAndLogsSuccess()
        {
            // Arrange
            var now = DateTime.UtcNow;
            _clock.Setup(c => c.UtcNow).Returns(now);

            var user = new User { Id = Guid.NewGuid(), Email = "u@example.com", PasswordHash = "hash" };
            _users.Setup(u => u.GetByEmailAsync(user.Email, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _hasher.Setup(h => h.Verify("correct", user.PasswordHash)).Returns(true);

            var clientId = Guid.NewGuid();
            var app = new ApplicationResult(Guid.NewGuid(), clientId, "app", true, Guid.NewGuid(), null, Array.Empty<string>());
            _applications.Setup(a => a.GetByClientIdAsync(clientId, It.IsAny<CancellationToken>())).ReturnsAsync(app);

            _org.Setup(o => o.IsUserInOrganizationAsync(user.Id, app.OrganizationId, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var grant = Grant.CreateAuthorizationCode(app.Id, "https://r", "ch", "S256", now);
            grant.Id = Guid.NewGuid();
            // grant.UserId is Guid.Empty initially -> handler will Authorize it
            _authStore.Setup(s => s.GetGrantId()).Returns(grant.Id);
            _grants.Setup(g => g.GetByIdAsync(grant.Id, It.IsAny<CancellationToken>())).ReturnsAsync(grant);
            _grants.Setup(g => g.UpdateAsync(It.IsAny<Grant>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask).Verifiable();

            var handler = CreateHandler();

            var cmd = new Command(clientId, user.Email, "correct", RedirectUri: "", CodeChallenge: "", CodeChallengeMethod: "", Scopes: "openid profile");

            // Act
            var result = await handler.Handle(cmd, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(grant.CodeHash, result.Value!.AuthorizationCode);
            _grants.Verify(g => g.UpdateAsync(It.Is<Grant>(x => x.UserId == user.Id), It.IsAny<CancellationToken>()), Times.Once);
            _authStore.Verify(s => s.Clear(), Times.Once);
            _audit.Verify(a => a.LogSuccessAsync(AuthEventTypes.LOGIN_SUCCESS), Times.Once);
        }

        [Fact]
        public async Task Handle_CommandLikeEmail_ReturnsFailureAndLogsFailure()
        {
            var handler = CreateHandler();

            var cmd = new Command(Guid.NewGuid(), "user@example.com && whoami", "Valid11!", "", "", "", "openid");

            var result = await handler.Handle(cmd, CancellationToken.None);

            Assert.True(result.IsFailure);
            _users.Verify(u => u.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            _audit.Verify(a => a.LogFailureAsync(AuthEventTypes.LOGIN_FAILURE), Times.Once);
        }

        [Fact]
        public async Task Handle_TooLongPassword_ReturnsFailureAndLogsFailure()
        {
            var handler = CreateHandler();

            var tooLongPassword = new string('a', 256);
            var cmd = new Command(Guid.NewGuid(), "user@example.com", tooLongPassword, "", "", "", "openid");

            var result = await handler.Handle(cmd, CancellationToken.None);

            Assert.True(result.IsFailure);
            _users.Verify(u => u.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            _audit.Verify(a => a.LogFailureAsync(AuthEventTypes.LOGIN_FAILURE), Times.Once);
        }

        [Fact]
        public async Task Handle_EmailWithNoise_SanitizesBeforeUserLookup()
        {
            var now = DateTime.UtcNow;
            _clock.Setup(c => c.UtcNow).Returns(now);

            var sanitizedEmail = "john.doe@example.com";
            var user = new User { Id = Guid.NewGuid(), Email = sanitizedEmail, PasswordHash = "hash" };
            _users.Setup(u => u.GetByEmailAsync(sanitizedEmail, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            _hasher.Setup(h => h.Verify("Valid11!", user.PasswordHash)).Returns(true);

            var clientId = Guid.NewGuid();
            var app = new ApplicationResult(Guid.NewGuid(), clientId, "app", true, Guid.NewGuid(), null, Array.Empty<string>());
            _applications.Setup(a => a.GetByClientIdAsync(clientId, It.IsAny<CancellationToken>())).ReturnsAsync(app);

            _org.Setup(o => o.IsUserInOrganizationAsync(user.Id, app.OrganizationId, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var grant = Grant.CreateAuthorizationCode(app.Id, "https://r", "ch", "S256", now);
            grant.Id = Guid.NewGuid();
            _authStore.Setup(s => s.GetGrantId()).Returns(grant.Id);
            _grants.Setup(g => g.GetByIdAsync(grant.Id, It.IsAny<CancellationToken>())).ReturnsAsync(grant);
            _grants.Setup(g => g.UpdateAsync(It.IsAny<Grant>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var handler = CreateHandler();
            var cmd = new Command(clientId, "  john.<doe>@example.com  ", "Valid11!", "", "", "", "openid");

            var result = await handler.Handle(cmd, CancellationToken.None);

            Assert.True(result.IsSuccess);
            _users.Verify(u => u.GetByEmailAsync(sanitizedEmail, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
