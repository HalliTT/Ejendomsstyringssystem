using AuthService.Application.Applications;
using AuthService.Application.Audit;
using AuthService.Application.Auth.Grants;
using AuthService.Application.Auth.Password;
using AuthService.Application.Auth.Verify;
using AuthService.Application.Common.Interfaces;
using AuthService.Application.Organizations;
using AuthService.Application.Users;
using AuthService.Domain.Audit;
using AuthService.Domain.Common;
using MediatR;

namespace AuthService.Application.Auth.Login
{
    public sealed class Handler : IRequestHandler<Command, Result<LoginResult>>
    {
        private readonly IUserRepository _users;
        private readonly IApplicationRepository _application;
        private readonly IGrantRepository _grantRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IClock _clock;
        private readonly IAuthorizationRequestStore _authStore;
        private readonly IOrganizationRepository _Organization;
        private readonly IAuditService _audit;

        public Handler(IUserRepository users, 
            IApplicationRepository applicationRepository, 
            IGrantRepository grantRepository, 
            IPasswordHasher passwordHasher, 
            IClock clock, 
            IAuthorizationRequestStore authStore,
            IOrganizationRepository organizationRepository,
            IAuditService audit)
        {
            _users = users;
            _application = applicationRepository;
            _grantRepository = grantRepository;
            _passwordHasher = passwordHasher;
            _clock = clock;
            _authStore = authStore;
            _Organization = organizationRepository;
            _audit = audit;
        }

        public async Task<Result<LoginResult>> Handle(Command request, CancellationToken ct)
        {
            if (!LoginInputGuard.TrySanitizeAndValidate(request.Email, request.Password, out var safeEmail, out var safePassword))
            {
                await _audit.LogFailureAsync(AuthEventTypes.LOGIN_FAILURE);
                return Result<LoginResult>.Failure();
            }

            // USER
            var user = await _users.GetByEmailAsync(safeEmail, ct);
            if (user is null)
            {
                await _audit.LogFailureAsync(AuthEventTypes.LOGIN_FAILURE);
                return Result<LoginResult>.Failure();
            }

            if (!_passwordHasher.Verify(safePassword, user.PasswordHash))
            {
                await _audit.LogFailureAsync(AuthEventTypes.LOGIN_FAILURE);
                return Result<LoginResult>.Failure();
            }

            // APPLICATION
            var application = await _application.GetByClientIdAsync(request.ClientId, ct);
            if (application is null)
            {
                await _audit.LogFailureAsync(AuthEventTypes.LOGIN_FAILURE);
                return Result<LoginResult>.Failure();
            }

            // ORGANIZATION
            var isUserInOrg = await _Organization.IsUserInOrganizationAsync(user.Id, application.OrganizationId, ct);

            // GRANT
            var grantId = _authStore.GetGrantId();
            if (grantId == null)
            {
                await _audit.LogFailureAsync(AuthEventTypes.LOGIN_FAILURE);
                return Result<LoginResult>.Failure();
            }

            var grant = await _grantRepository.GetByIdAsync(grantId, ct);
            if (grant == null)
            {
                await _audit.LogFailureAsync(AuthEventTypes.LOGIN_FAILURE);
                return Result<LoginResult>.Failure();
            }

            if (grant.UserId != Guid.Empty)
            {
                await _audit.LogFailureAsync(AuthEventTypes.LOGIN_FAILURE);
                return Result<LoginResult>.Failure();
            }

            grant.EnsureValid(_clock.UtcNow);
            grant.Authorize(user.Id, _clock.UtcNow);

            grant.Scopes = request.Scopes;
            await _grantRepository.UpdateAsync(grant, ct);
            _authStore.Clear();

            var scopeList = request.Scopes?
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .ToList()
                .AsReadOnly() ?? new List<string>().AsReadOnly();

            var appInfo = new ApplicationInfo(application.Name ?? string.Empty, scopeList, grantId ?? Guid.Empty);

            await _audit.LogSuccessAsync(AuthEventTypes.LOGIN_SUCCESS);

            return Result<LoginResult>.Success(
                new LoginResult(grant.CodeHash!, isUserInOrg, appInfo, scopeList)
            );
        }
    }
}
