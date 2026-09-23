using ESS.Application.Bookings.Get;
using ESS.Application.Common.Interface;
using ESS.Application.Properties;
using ESS.Application.Rentals;
using ESS.Application.Tenants;
using ESS.Application.Users;
using MediatR;

namespace ESS.Application.Bookings.Update
{
    public sealed class Handler : IRequestHandler<Command, BookingMutationResult>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRentalOptionRepository _rentalOptionRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly ITenantRepository _tenantRepository;
        private readonly IAppUserRepository _appUserRepository;
        private readonly ICurrentUser _currentUser;

        public Handler(
            IBookingRepository bookingRepository,
            IRentalOptionRepository rentalOptionRepository,
            IPropertyRepository propertyRepository,
            ITenantRepository tenantRepository,
            IAppUserRepository appUserRepository,
            ICurrentUser currentUser)
        {
            _bookingRepository = bookingRepository;
            _rentalOptionRepository = rentalOptionRepository;
            _propertyRepository = propertyRepository;
            _tenantRepository = tenantRepository;
            _appUserRepository = appUserRepository;
            _currentUser = currentUser;
        }

        public async Task<BookingMutationResult> Handle(Command command, CancellationToken ct)
        {
            if (_currentUser.UserId is null)
                return new BookingMutationResult(null, true, false);

            var appUser = await _appUserRepository.GetByIdAsync(_currentUser.UserId.Value, ct);
            if (appUser?.OwnerId is null)
                return new BookingMutationResult(null, true, false);

            var existing = await _bookingRepository.GetByIdAsync(command.BookingId, ct);
            if (existing is null)
                return new BookingMutationResult(null, true, false);

            var existingRentalOption = await _rentalOptionRepository.GetByIdAsync(existing.RentalOptionId, ct);
            if (existingRentalOption is null)
                return new BookingMutationResult(null, true, false);

            var existingProperty = await _propertyRepository.GetByIdAsync(existingRentalOption.PropertyId, ct);
            if (existingProperty is null || existingProperty.OwnerId != appUser.OwnerId)
                return new BookingMutationResult(null, true, false);

            var targetRentalOption = await _rentalOptionRepository.GetByIdAsync(command.RentalOptionId, ct);
            if (targetRentalOption is null)
                return new BookingMutationResult(null, true, false);

            var targetProperty = await _propertyRepository.GetByIdAsync(targetRentalOption.PropertyId, ct);
            if (targetProperty is null || targetProperty.OwnerId != appUser.OwnerId)
                return new BookingMutationResult(null, true, false);

            var tenant = await _tenantRepository.GetByIdAsync(command.TenantId, ct);
            if (tenant is null)
                return new BookingMutationResult(null, true, false);

            var hasOverlap = await _bookingRepository.HasOverlapAsync(
                command.RentalOptionId, command.StartDate, command.EndDate, command.BookingId, ct);
            if (hasOverlap)
                return new BookingMutationResult(null, false, true);

            var updated = await _bookingRepository.UpdateAsync(
                command.BookingId, command.RentalOptionId, command.TenantId, command.StartDate, command.EndDate, ct);
            if (updated is null)
                return new BookingMutationResult(null, true, false);

            return new BookingMutationResult(
                BookingDtoFactory.Create(updated, targetRentalOption, targetProperty, tenant), false, false);
        }
    }
}
