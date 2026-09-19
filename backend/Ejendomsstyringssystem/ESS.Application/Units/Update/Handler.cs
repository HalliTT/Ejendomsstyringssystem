using ESS.Application.Common.Interface;
using ESS.Application.Properties;
using ESS.Application.Units.Get;
using ESS.Application.Users;
using MediatR;

namespace ESS.Application.Units.Update
{
    public sealed class Handler : IRequestHandler<Command, UnitDto?>
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

        public async Task<UnitDto?> Handle(Command command, CancellationToken ct)
        {
            if (_currentUser.UserId is null)
                return null;

            var appUser = await _appUserRepository.GetByIdAsync(_currentUser.UserId.Value, ct);
            if (appUser?.OwnerId is null)
                return null;

            var existingUnit = await _unitRepository.GetByIdAsync(command.UnitId, ct);
            if (existingUnit is null)
                return null;

            var property = await _propertyRepository.GetByIdAsync(existingUnit.PropertyId, ct);
            if (property is null || property.OwnerId != appUser.OwnerId)
                return null;

            var updated = await _unitRepository.UpdateAsync(
                command.UnitId,
                command.Name,
                command.Description,
                command.Status,
                ct);

            if (updated is null)
                return null;

            return new UnitDto(
                updated.Id,
                updated.PropertyId,
                updated.Name ?? "",
                updated.Description ?? "",
                updated.Status
            );
        }
    }
}
