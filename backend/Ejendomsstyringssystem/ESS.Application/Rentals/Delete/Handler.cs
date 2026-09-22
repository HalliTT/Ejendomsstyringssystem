using ESS.Application.Common.Interface;
using ESS.Application.Properties;
using ESS.Application.Users;
using MediatR;

namespace ESS.Application.Rentals.Delete
{
    public sealed class Handler : IRequestHandler<Command, bool>
    {
        private readonly IRentalOptionRepository _rentalOptionRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IAppUserRepository _appUserRepository;
        private readonly ICurrentUser _currentUser;

        public Handler(
            IRentalOptionRepository rentalOptionRepository,
            IPropertyRepository propertyRepository,
            IAppUserRepository appUserRepository,
            ICurrentUser currentUser)
        {
            _rentalOptionRepository = rentalOptionRepository;
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

            var existing = await _rentalOptionRepository.GetByIdAsync(command.RentalOptionId, ct);
            if (existing is null)
                return false;

            var property = await _propertyRepository.GetByIdAsync(existing.PropertyId, ct);
            if (property is null || property.OwnerId != appUser.OwnerId)
                return false;

            return await _rentalOptionRepository.DeleteAsync(command.RentalOptionId, ct);
        }
    }
}
