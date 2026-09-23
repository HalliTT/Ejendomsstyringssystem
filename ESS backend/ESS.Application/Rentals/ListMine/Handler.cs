using ESS.Application.Common.Interface;
using ESS.Application.Properties;
using ESS.Application.Users;
using ESS.Domain.Properties;
using MediatR;

namespace ESS.Application.Rentals.ListMine
{
    public sealed class Handler : IRequestHandler<Query, IReadOnlyList<RentalOptionSummaryDto>>
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

        public async Task<IReadOnlyList<RentalOptionSummaryDto>> Handle(Query request, CancellationToken ct)
        {
            if (_currentUser.UserId is null)
                return new List<RentalOptionSummaryDto>();

            var appUser = await _appUserRepository.GetByIdAsync(_currentUser.UserId.Value, ct);
            if (appUser?.OwnerId is null)
                return new List<RentalOptionSummaryDto>();

            var rentalOptions = await _rentalOptionRepository.ListByOwnerIdAsync(appUser.OwnerId.Value, ct);

            var propertiesById = new Dictionary<Guid, Property>();
            var result = new List<RentalOptionSummaryDto>();

            foreach (var rentalOption in rentalOptions)
            {
                if (!propertiesById.TryGetValue(rentalOption.PropertyId, out var property))
                {
                    property = await _propertyRepository.GetByIdAsync(rentalOption.PropertyId, ct);
                    if (property is not null)
                        propertiesById[rentalOption.PropertyId] = property;
                }

                if (property is null)
                    continue;

                result.Add(new RentalOptionSummaryDto(
                    rentalOption.Id,
                    rentalOption.Name ?? "",
                    rentalOption.MonthlyRent,
                    rentalOption.Status,
                    property.Id,
                    property.Name ?? ""));
            }

            return result;
        }
    }
}
