using AuthService.Application.Auth.Logout;
using AuthService.Application.Auth.Session;
using AuthService.Application.Common.Interfaces;
using AuthService.Domain.Sessions;
using Moq;
using Xunit;

namespace AuthService.Test.Handlers
{
    public class LogoutTests
    {
        [Fact]
        public async Task Handle_SessionNotFound_ReturnsSuccessAndDoesNotUpdate()
        {
            var sessionId = Guid.NewGuid();
            var repo = new Mock<ISessionRepository>();
            repo
                .Setup(r => r.GetSessionById(It.Is<Guid>(g => g == sessionId), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Session?)null);

            var clock = new Mock<IClock>();

            var handler = new Handler(repo.Object, clock.Object);

            // Act
            var result = await handler.Handle(new Command(sessionId), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            repo.Verify(r => r.UpdateAsync(It.IsAny<Session>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
