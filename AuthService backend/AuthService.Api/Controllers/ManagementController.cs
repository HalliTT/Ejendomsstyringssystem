using AuthService.Api.Security;
using AuthService.Application.Users;
using AuthService.Contracts.Management;
using AuthService.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthService.Api.Controllers
{
    [ApiController]
    [Route("management")]
    [Authorize(Policy = ScopePolicies.ManagementAccess)]
    public sealed class ManagementController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;

        public ManagementController(IMediator mediator, IUserRepository userRepository)
        {
            _mediator = mediator;
            _userRepository = userRepository;
        }

        //[HttpGet("hasher")]
        //public async Task<IActionResult> Hash(CancellationToken ct)
        //{
        //    var hasher = new PasswordHasher<object>();
        //    var hash = hasher.HashPassword(null!, "Password123!");
        //    Console.WriteLine(hash);
        //    return Ok();
        //}


        [HttpGet("organizations")]
        [Authorize(Policy = ScopePolicies.OrganizationRead)]
        public async Task<IActionResult> ListOrganizations(CancellationToken ct)
        {
            return Ok(await _mediator.Send(new Application.Organizations.List.Query(), ct));
        }

        [HttpPost("organizations")]
        [Authorize(Policy = ScopePolicies.OrganizationWrite)]
        public async Task<IActionResult> CreateOrganization([FromBody] CreateOrganizationRequest request, CancellationToken ct)
        {
            var command = new Application.Organizations.Create.Command(
                request.Name,
                request.Description,
                request.Slug,
                request.OwnerId);

            return Ok(await _mediator.Send(command, ct));
        }

        [HttpPut("organizations/{organizationId:guid}")]
        [Authorize(Policy = ScopePolicies.OrganizationWrite)]
        public async Task<IActionResult> UpdateOrganization(
            [FromRoute] Guid organizationId,
            [FromBody] UpdateOrganizationRequest request,
            CancellationToken ct)
        {
            var command = new Application.Organizations.Update.Command(
                organizationId,
                request.Name,
                request.Description,
                request.Slug,
                request.OwnerId);

            return Ok(await _mediator.Send(command, ct));
        }

        [HttpDelete("organizations/{organizationId:guid}")]
        [Authorize(Policy = ScopePolicies.OrganizationWrite)]
        public async Task<IActionResult> DeleteOrganization([FromRoute] Guid organizationId, CancellationToken ct)
        {
            if (!TryGetCurrentUserId(out var deletedBy))
            {
                return UnauthorizedResponse();
            }

            return Ok(await _mediator.Send(new Application.Organizations.Delete.Command(organizationId, deletedBy), ct));
        }

        [HttpGet("organizations/{organizationId:guid}/applications")]
        [Authorize(Policy = ScopePolicies.ApplicationRead)]
        public async Task<IActionResult> ListApplications([FromRoute] Guid organizationId, CancellationToken ct)
        {
            return Ok(await _mediator.Send(new Application.Applications.ListByOrganization.Query(organizationId), ct));
        }

        [HttpPost("organizations/{organizationId:guid}/applications")]
        [Authorize(Policy = ScopePolicies.ApplicationWrite)]
        public async Task<IActionResult> CreateApplication(
            [FromRoute] Guid organizationId,
            [FromBody] CreateApplicationRequest request,
            CancellationToken ct)
        {
            var command = new Application.Applications.Create.Command(
                organizationId,
                request.Name,
                request.RedirectUris);

            return Ok(await _mediator.Send(command, ct));
        }

        [HttpPut("organizations/{organizationId:guid}/applications/{applicationId:guid}")]
        [Authorize(Policy = ScopePolicies.ApplicationWrite)]
        public async Task<IActionResult> UpdateApplication(
            [FromRoute] Guid organizationId,
            [FromRoute] Guid applicationId,
            [FromBody] UpdateApplicationRequest request,
            CancellationToken ct)
        {
            var command = new Application.Applications.Update.Command(
                organizationId,
                applicationId,
                request.Name,
                request.IsEnabled);

            return Ok(await _mediator.Send(command, ct));
        }

        [HttpDelete("organizations/{organizationId:guid}/applications/{applicationId:guid}")]
        [Authorize(Policy = ScopePolicies.ApplicationWrite)]
        public async Task<IActionResult> DeleteApplication(
            [FromRoute] Guid organizationId,
            [FromRoute] Guid applicationId,
            CancellationToken ct)
        {
            if (!TryGetCurrentUserId(out var deletedBy))
            {
                return UnauthorizedResponse();
            }

            return Ok(await _mediator.Send(new Application.Applications.Delete.Command(organizationId, applicationId, deletedBy), ct));
        }

        [HttpGet("applications/{applicationId:guid}/redirect-uris")]
        [Authorize(Policy = ScopePolicies.RedirectUriRead)]
        public async Task<IActionResult> ListRedirectUris([FromRoute] Guid applicationId, CancellationToken ct)
        {
            return Ok(await _mediator.Send(new Application.Applications.RedirectUris.List.Query(applicationId), ct));
        }

        [HttpPost("applications/{applicationId:guid}/redirect-uris")]
        [Authorize(Policy = ScopePolicies.RedirectUriWrite)]
        public async Task<IActionResult> AddRedirectUri(
            [FromRoute] Guid applicationId,
            [FromBody] AddRedirectUriRequest request,
            CancellationToken ct)
        {
            var command = new Application.Applications.RedirectUris.Add.Command(applicationId, request.RedirectUri);
            return Ok(await _mediator.Send(command, ct));
        }

        [HttpDelete("applications/{applicationId:guid}/redirect-uris/{redirectUriId:guid}")]
        [Authorize(Policy = ScopePolicies.RedirectUriWrite)]
        public async Task<IActionResult> DeleteRedirectUri(
            [FromRoute] Guid applicationId,
            [FromRoute] Guid redirectUriId,
            CancellationToken ct)
        {
            var command = new Application.Applications.RedirectUris.Delete.Command(applicationId, redirectUriId);
            return Ok(await _mediator.Send(command, ct));
        }

        [HttpGet("events/recent")]
        [Authorize]
        public async Task<IActionResult> GetRecentAuditEvents([FromQuery] int take = 10, CancellationToken ct = default)
        {
            var query = new Application.Audit.GetRecentAuditEvents.Query(take);
            return Ok(await _mediator.Send(query, ct));
        }

        [HttpGet("users")]
        [Authorize(Policy = ScopePolicies.OrganizationRead)]
        public async Task<IActionResult> ListUsers(CancellationToken ct)
        {
            var user = await _mediator.Send(new Application.Users.GetAllUsers.Query(), ct);
            return Ok(user);
        }

        //[HttpPost("users")]
        //[Authorize(Policy = ScopePolicies.OrganizationWrite)]
        //public async Task<IActionResult> CreateUsers([FromBody] CreateUserRequest request, CancellationToken ct)
        //{
        //    var command = new Application.Users.Create.Command(
        //        request.Email,
        //        request.FirstName,
        //        request.LastName,
        //        request.Password,
        //        request.CreatedBy,
        //        request.AddressId,
        //        request.IsAdmin);

        //    return Ok(await _mediator.Send(command, ct));
        //}

        [HttpPut("users/{usersId:guid}")]
        [Authorize(Policy = ScopePolicies.OrganizationWrite)]
        public async Task<IActionResult> UpdateUsers(
            [FromRoute] Guid organizationId,
            [FromBody] UpdateOrganizationRequest request,
            CancellationToken ct)
        {
            var command = new Application.Organizations.Update.Command(
                organizationId,
                request.Name,
                request.Description,
                request.Slug,
                request.OwnerId);

            return Ok(await _mediator.Send(command, ct));
        }



        [HttpGet("verify-access")]
        public async Task<IActionResult> VerifyAccess(CancellationToken ct)
        {
            if (!TryGetCurrentUserId(out var userId))
            {
                return UnauthorizedResponse();
            }

            var user = await _userRepository.GetByIdAsync(userId, ct);
            if (user is null)
            {
                return UnauthorizedResponse();
            }

            if (!user.IsAdmin)
            {
                return ForbiddenResponse();
            }

            return Ok(Result<object>.Success(new { hasAccess = true }));
        }

        private bool TryGetCurrentUserId(out Guid userId)
        {
            var value = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            return Guid.TryParse(value, out userId);
        }
    }
}
