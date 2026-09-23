using ESS.Application.Bookings.Get;
using ESS.Application.Common.Interface;
using ESS.Application.Properties;
using ESS.Application.Rentals;
using ESS.Application.Tenants;
using ESS.Application.Users;
using ESS.Domain.Properties;
using ESS.Domain.Rentals;
using ESS.Domain.Tenants;
using MediatR;

namespace ESS.Application.Bookings.List
{
    public sealed class Handler : IRequestHandler<Query, IReadOnlyList<BookingDto>>
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

        public async Task<IReadOnlyList<BookingDto>> Handle(Query request, CancellationToken ct)
        {
            if (_currentUser.UserId is null)
                return new List<BookingDto>();

            var appUser = await _appUserRepository.GetByIdAsync(_currentUser.UserId.Value, ct);
            if (appUser?.OwnerId is null)
                return new List<BookingDto>();

            var bookings = await _bookingRepository.ListByOwnerIdAsync(appUser.OwnerId.Value, ct);

            var rentalOptionsById = new Dictionary<Guid, RentalOption>();
            var propertiesById = new Dictionary<Guid, Property>();
            var tenantsById = new Dictionary<Guid, Tenant>();
            var result = new List<BookingDto>();

            foreach (var booking in bookings)
            {
                if (!rentalOptionsById.TryGetValue(booking.RentalOptionId, out var rentalOption))
                {
                    rentalOption = await _rentalOptionRepository.GetByIdAsync(booking.RentalOptionId, ct);
                    if (rentalOption is not null)
                        rentalOptionsById[booking.RentalOptionId] = rentalOption;
                }
                if (rentalOption is null)
                    continue;

                if (!propertiesById.TryGetValue(rentalOption.PropertyId, out var property))
                {
                    property = await _propertyRepository.GetByIdAsync(rentalOption.PropertyId, ct);
                    if (property is not null)
                        propertiesById[rentalOption.PropertyId] = property;
                }
                if (property is null)
                    continue;

                if (!tenantsById.TryGetValue(booking.TenantId, out var tenant))
                {
                    tenant = await _tenantRepository.GetByIdAsync(booking.TenantId, ct);
                    if (tenant is not null)
                        tenantsById[booking.TenantId] = tenant;
                }
                if (tenant is null)
                    continue;

                result.Add(BookingDtoFactory.Create(booking, rentalOption, property, tenant));
            }

            return result.OrderByDescending(b => b.StartDate).ToList();
        }
    }
}
