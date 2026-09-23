using ESS.Application.Bookings.Get;
using ESS.Application.Common.Interface;
using ESS.Application.Properties;
using ESS.Application.Rentals;
using ESS.Application.Tenants;
using ESS.Application.Users;
using MediatR;

namespace ESS.Application.Bookings.Create
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

            var rentalOption = await _rentalOptionRepository.GetByIdAsync(command.RentalOptionId, ct);
            if (rentalOption is null)
                return new BookingMutationResult(null, true, false);

            var property = await _propertyRepository.GetByIdAsync(rentalOption.PropertyId, ct);
            if (property is null || property.OwnerId != appUser.OwnerId)
                return new BookingMutationResult(null, true, false);

            var tenant = await _tenantRepository.GetByIdAsync(command.TenantId, ct);
            if (tenant is null)
                return new BookingMutationResult(null, true, false);

            var hasOverlap = await _bookingRepository.HasOverlapAsync(
                command.RentalOptionId, command.StartDate, command.EndDate, null, ct);
            if (hasOverlap)
                return new BookingMutationResult(null, false, true);

            var created = await _bookingRepository.CreateAsync(
                command.RentalOptionId, command.TenantId, command.StartDate, command.EndDate, ct);
            if (created is null)
                return new BookingMutationResult(null, true, false);

            return new BookingMutationResult(
                BookingDtoFactory.Create(created, rentalOption, property, tenant), false, false);
        }
    }
}
