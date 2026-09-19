using ESS.Application.Common.Interface;
using ESS.Application.Properties;
using ESS.Application.Users;
using MediatR;

namespace ESS.Application.Units.Delete
{
    public sealed class Handler : IRequestHandler<Command, bool>
    {
        private readonly IUnitRepository _unitRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IAppUserRepository _appUserRepository;
        private readonly ICurrentUser _currentUser;

        public Handler(IUnitRepository unitRepository, IPropertyRepository propertyRepository, IAppUserRepository appUserRepository, ICurrentUser currentUser)
        {
            _unitRepository = unitRepository;
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

            var existing = await _unitRepository.GetByIdAsync(command.UnitId, ct);
            if (existing is null)
                return false;

            var property = await _propertyRepository.GetByIdAsync(existing.PropertyId, ct);
            if (property is null || property.OwnerId != appUser.OwnerId)
                return false;

            return await _unitRepository.DeleteAsync(command.UnitId, ct);
        }
    }
}
