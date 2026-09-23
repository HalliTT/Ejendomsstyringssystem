using ESS.Application.Common.Interface;
using ESS.Application.Properties;
using ESS.Application.Rentals;
using ESS.Application.Tenants;
using ESS.Application.Users;
using MediatR;

namespace ESS.Application.Bookings.Get
{
    public sealed class Handler : IRequestHandler<Query, BookingDto?>
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

        public async Task<BookingDto?> Handle(Query request, CancellationToken ct)
        {
            if (_currentUser.UserId is null)
                return null;

            var appUser = await _appUserRepository.GetByIdAsync(_currentUser.UserId.Value, ct);
            if (appUser?.OwnerId is null)
                return null;

            var booking = await _bookingRepository.GetByIdAsync(request.BookingId, ct);
            if (booking is null)
                return null;

            var rentalOption = await _rentalOptionRepository.GetByIdAsync(booking.RentalOptionId, ct);
            if (rentalOption is null)
                return null;

            var property = await _propertyRepository.GetByIdAsync(rentalOption.PropertyId, ct);
            if (property is null || property.OwnerId != appUser.OwnerId)
                return null;

            var tenant = await _tenantRepository.GetByIdAsync(booking.TenantId, ct);
            if (tenant is null)
                return null;

            return BookingDtoFactory.Create(booking, rentalOption, property, tenant);
        }
    }
}
