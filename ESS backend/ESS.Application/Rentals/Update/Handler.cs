using ESS.Application.Common.Interface;
using ESS.Application.Properties;
using ESS.Application.Rentals.Get;
using ESS.Application.Units;
using ESS.Application.Users;
using MediatR;

namespace ESS.Application.Rentals.Update
{
    public sealed class Handler : IRequestHandler<Command, RentalOptionDto?>
    {
        private readonly IRentalOptionRepository _rentalOptionRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IUnitRepository _unitRepository;
        private readonly IAppUserRepository _appUserRepository;
        private readonly ICurrentUser _currentUser;

        public Handler(
            IRentalOptionRepository rentalOptionRepository,
            IPropertyRepository propertyRepository,
            IUnitRepository unitRepository,
            IAppUserRepository appUserRepository,
            ICurrentUser currentUser)
        {
            _rentalOptionRepository = rentalOptionRepository;
            _propertyRepository = propertyRepository;
            _unitRepository = unitRepository;
            _appUserRepository = appUserRepository;
            _currentUser = currentUser;
        }

        public async Task<RentalOptionDto?> Handle(Command command, CancellationToken ct)
        {
            if (_currentUser.UserId is null)
                return null;

            var appUser = await _appUserRepository.GetByIdAsync(_currentUser.UserId.Value, ct);
            if (appUser?.OwnerId is null)
                return null;

            var existing = await _rentalOptionRepository.GetByIdAsync(command.RentalOptionId, ct);
            if (existing is null)
                return null;

            var property = await _propertyRepository.GetByIdAsync(existing.PropertyId, ct);
            if (property is null || property.OwnerId != appUser.OwnerId)
                return null;

            var propertyUnitIds = (await _unitRepository.ListByPropertyIdAsync(existing.PropertyId, ct))
                .Select(u => u.Id)
                .ToHashSet();
            if (command.UnitIds.Any(id => !propertyUnitIds.Contains(id)))
                return null;

            var updated = await _rentalOptionRepository.UpdateAsync(
                command.RentalOptionId,
                command.Name,
                command.MonthlyRent,
                command.Status,
                command.UnitIds,
                ct);

            if (updated is null)
                return null;

            return new RentalOptionDto(
                updated.Id,
                updated.PropertyId,
                updated.Name ?? "",
                updated.MonthlyRent,
                updated.Status,
                command.UnitIds.Distinct().ToList()
            );
        }
    }
}
