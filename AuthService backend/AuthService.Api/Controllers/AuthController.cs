using AuthService.Application.Users;
using AuthService.Contracts.Auth;
using AuthService.Contracts.Grants;
using AuthService.Domain.Common;
using AuthService.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthService.Api.Controllers
{
    [ApiController]
    [Route("auth/")]
    public class AuthController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;


        public AuthController(IMediator mediator, IUserRepository userRepository)
        {
            _mediator = mediator;
            _userRepository = userRepository;
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify(
            [FromBody] VerifyRequest request,
            CancellationToken ct)
        {
            var result = await _mediator.Send(
                new Application.Auth.Verify.ClientVerification.Command(
                    request.ClientId,
                    request.RedirectUri,
                    request.ResponseType,
                    request.CodeChallenge,
                    request.CodeChallengeMethod,
                    request.State,
                    request.Scopes
                    ),
                ct);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest request,
            CancellationToken ct)
        {
            var result = await _mediator.Send(
                new Application.Auth.Login.Command(
                    request.ClientId,
                    request.Email,
                    request.Password,
                    request.RedirectUri,
                    request.CodeChallenge,
                    request.CodeChallengeMethod,
                    request.Scopes),
                ct);

            return Ok(result);
        }

        [HttpPost("consent")]
        public async Task<IActionResult> Consent(
            [FromBody] ConsentRequest request,
            CancellationToken ct)
        {
            var result = await _mediator.Send(
                new Application.Auth.Grants.ConsentApplication.Command(
                    request.GrantId,
                    request.Approved),
                ct);

            return Ok(result);
        }

        [HttpPost("token")]
        public async Task<IActionResult> Token(
            [FromBody] SessionRequest request,
            CancellationToken ct)
        {
            var result = await _mediator.Send(
                new Application.Auth.Session.GenerateSession.Command(
                    request.code,
                    request.codeVerifier,
                    request.deviceId
                    ),
                ct);

            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(
            [FromBody] RefreshRequest request,
            CancellationToken ct)
        {
            var result = await _mediator.Send(
                new Application.Auth.Session.Refresh.Command(
                    request.RefreshToken,
                    request.ClientId,
                    request.RedirectUri,
                    request.CodeChallenge,
                    request.CodeChallengeMethod,
                    request.Scopes
                    ),
                ct);

            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(
            [FromBody] LogoutRequest request,
            CancellationToken ct)
        {
            await _mediator.Send(new Application.Auth.Logout.Command(request.sessionId), ct);
            return Ok(Result.Success(), "Logout completed successfully");
        }

        [HttpPost("logout-all")]
        public async Task<IActionResult> LogoutAll(
            [FromBody] LogoutRequestAll request,
            CancellationToken ct)
        {
            await _mediator.Send(new Application.Auth.Logout.AllDevices.Command(request.userId), ct);
            return Ok(Result.Success(), "Logout completed successfully on all devices");
        }

        [HttpGet("userinfo")]
        [Authorize]
        public async Task<IActionResult> UserInfo(CancellationToken ct)
        {
            var value = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

            if (!Guid.TryParse(value, out var userId))
                return UnauthorizedResponse();

            var user = await _userRepository.GetByIdAsync(userId, ct);

            if (user is null)
                return UnauthorizedResponse();

            var scopes = User.FindAll("scope").Select(c => c.Value);

            return Ok(Result<object>.Success(new
            {
                userId = user.Id,
                email = user.Email,
                firstName = user.FirstName,
                lastName = user.LastName,
                displayName = user.DisplayName,
                isAdmin = user.IsAdmin,
                avatar = user.Avatar,
                scopes,
            }));

        }
    }
}
