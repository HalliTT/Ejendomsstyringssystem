using ESS.Application.Common.Interface;
using ESS.Application.Properties.Get;
using ESS.Application.Rentals;
using ESS.Application.Rentals.Get;
using ESS.Application.Units;
using ESS.Application.Units.List;
using ESS.Application.Users;
using MediatR;

namespace ESS.Application.Properties.Update
{
    public sealed class Handler : IRequestHandler<Command, PropertyDto?>
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

        public async Task<PropertyDto?> Handle(Command command, CancellationToken ct)
        {
            if (_currentUser.UserId is null)
                return null;

            var appUser = await _appUserRepository.GetByIdAsync(_currentUser.UserId.Value, ct);
            if (appUser?.OwnerId is null)
                return null;

            var existing = await _propertyRepository.GetByIdAsync(command.PropertyId, ct);
            if (existing is null || existing.OwnerId != appUser.OwnerId)
                return null;

            var updated = await _propertyRepository.UpdateAsync(
                command.PropertyId,
                command.Name,
                command.Address,
                command.City,
                command.Country,
                command.Description,
                ct);

            if (updated is null)
                return null;

            var units = await _unitRepository.ListByPropertyIdAsync(command.PropertyId, ct);

            var rentalOptions = await _rentalOptionRepository.ListByPropertyIdAsync(command.PropertyId, ct);
            var unitIdsByOption = await _rentalOptionRepository.GetUnitIdsByRentalOptionIdsAsync(
                rentalOptions.Select(r => r.Id).ToList(), ct);

            return new PropertyDto(
                updated.Id,
                updated.Name ?? "",
                updated.Address ?? "",
                updated.City ?? "",
                updated.Country ?? "",
                updated.Description ?? "",
                units.Count(u => u.Status == Domain.Units.UnitStatus.Occupied),
                units.Count,
                units.Select(u => new UnitListItemDto(u.Id, u.Name ?? "", u.Description ?? "", u.Status)).ToList(),
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
