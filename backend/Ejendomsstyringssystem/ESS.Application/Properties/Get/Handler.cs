using ESS.Application.Common.Interface;
using ESS.Application.Units;
using ESS.Application.Units.List;
using ESS.Application.Users;
using MediatR;

namespace ESS.Application.Properties.Get
{
    public sealed class Handler : IRequestHandler<Query, PropertyDto?>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IUnitRepository _unitRepository;
        private readonly IAppUserRepository _appUserRepository;
        private readonly ICurrentUser _currentUser;

        public Handler(IPropertyRepository propertyRepository, IUnitRepository unitRepository, IAppUserRepository appUserRepository, ICurrentUser currentUser)
        {
            _propertyRepository = propertyRepository;
            _unitRepository = unitRepository;
            _appUserRepository = appUserRepository;
            _currentUser = currentUser;
        }

        public async Task<PropertyDto?> Handle(Query request, CancellationToken ct)
        {
            var property = await _propertyRepository.GetByIdAsync(request.PropertyId, ct);

            if (property is null)
            {
                return null;
            }

            if (_currentUser is null)
                throw new UnauthorizedAccessException();

            var appUser = await _appUserRepository.GetByIdAsync(_currentUser.UserId.Value, ct);
            if (property.OwnerId != appUser.OwnerId)
                return null;

            var units = await  _unitRepository.ListByPropertyIdAsync(request.PropertyId, ct);

            var totalUnits = units.Count;
            var occupiedUnits = units.Count(u => u.Status == Domain.Units.UnitStatus.Occupied);

            return new PropertyDto(
                property.Id,
                property.Name ?? "",
                property.Address ?? "",
                property.City ?? "",
                property.Country ?? "",
                property.Description ?? "",
                occupiedUnits,
                totalUnits,
                units.Select(u => new UnitListItemDto(
                    u.Id,
                    u.Name ?? "",
                    u.Description ?? "",
                    u.Status))
                .ToList()
            );
        }
    }
}
