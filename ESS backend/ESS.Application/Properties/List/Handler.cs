using ESS.Application.Common.Interface;
using ESS.Application.Units;
using ESS.Application.Users;
using MediatR;

namespace ESS.Application.Properties.List
{
    public sealed class Handler : IRequestHandler<Query, IReadOnlyList<PropertyListItemDto>>
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

        public async Task<IReadOnlyList<PropertyListItemDto>> Handle(Query request, CancellationToken ct)
        {
            if (_currentUser.UserId is null)
                return Array.Empty<PropertyListItemDto>();

            var appUser = await _appUserRepository.GetByIdAsync(_currentUser.UserId.Value, ct);
            if (appUser?.OwnerId is null)
                return Array.Empty<PropertyListItemDto>();

            var properties = await _propertyRepository.ListAsync(appUser.OwnerId.Value, ct);
            var units = await _unitRepository.ListAsync(ct);
            var unitByProperty = units.ToLookup(u => u.PropertyId);


            return properties
                .Select(property =>
                {
                    var propertyUnits = unitByProperty[property.Id];
                    return new PropertyListItemDto(
                        property.Id,
                        property.Name,
                        property.Address,
                        propertyUnits.Count(u => u.Status == Domain.Units.UnitStatus.Occupied),
                        propertyUnits.Count());
                })
                .ToList();

        }
    }
}
