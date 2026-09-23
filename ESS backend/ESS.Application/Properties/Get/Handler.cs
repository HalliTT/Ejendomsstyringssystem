using ESS.Application.Common.Interface;
using ESS.Application.Rentals;
using ESS.Application.Rentals.Get;
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
        private readonly IRentalOptionRepository _rentalOptionRepository;
        private readonly IAppUserRepository _appUserRepository;
        private readonly ICurrentUser _currentUser;

        public Handler(
            IPropertyRepository propertyRepository,
            IUnitRepository unitRepository,
            IRentalOptionRepository rentalOptionRepository,
            IAppUserRepository appUserRepository,
            ICurrentUser currentUser)
        {
            _propertyRepository = propertyRepository;
            _unitRepository = unitRepository;
            _rentalOptionRepository = rentalOptionRepository;
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

            if (_currentUser.UserId is null)
                return null;

            var appUser = await _appUserRepository.GetByIdAsync(_currentUser.UserId.Value, ct);
            if (appUser?.OwnerId is null || property.OwnerId != appUser.OwnerId)
                return null;

            var units = await  _unitRepository.ListByPropertyIdAsync(request.PropertyId, ct);

            var totalUnits = units.Count;
            var occupiedUnits = units.Count(u => u.Status == Domain.Units.UnitStatus.Occupied);

            var rentalOptions = await _rentalOptionRepository.ListByPropertyIdAsync(request.PropertyId, ct);
            var unitIdsByOption = await _rentalOptionRepository.GetUnitIdsByRentalOptionIdsAsync(
                rentalOptions.Select(r => r.Id).ToList(), ct);

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
                .ToList(),
                rentalOptions.Select(r => new RentalOptionDto(
                    r.Id,
                    r.PropertyId,
                    r.Name ?? "",
                    r.MonthlyRent,
                    r.Status,
                    unitIdsByOption.TryGetValue(r.Id, out var unitIds) ? unitIds : new List<Guid>()))
                .ToList()
            );
        }
    }
}
