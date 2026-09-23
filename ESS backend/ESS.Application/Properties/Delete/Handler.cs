using ESS.Application.Common.Interface;
using ESS.Application.Users;
using MediatR;

namespace ESS.Application.Properties.Delete
{
    public sealed class Handler : IRequestHandler<Command, bool>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IAppUserRepository _appUserRepository;
        private readonly ICurrentUser _currentUser;

        public Handler(
            IPropertyRepository propertyRepository,
            IAppUserRepository appUserRepository,
            ICurrentUser currentUser)
        {
            _propertyRepository = propertyRepository;
            _appUserRepository = appUserRepository;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(Command command, CancellationToken ct)
        {
            if (_currentUser.UserId is null)
                return false;

            var appUser = await _appUserRepository.GetByIdAsync(_currentUser.UserId.Value, ct);
            if (appUser?.OwnerId is null)
                return false;

            var existing = await _propertyRepository.GetByIdAsync(command.PropertyId, ct);
            if (existing is null || existing.OwnerId != appUser.OwnerId)
                return false;

            return await _propertyRepository.DeleteAsync(command.PropertyId, ct);
        }
    }
}
