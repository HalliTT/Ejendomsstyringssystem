using ESS.Application.Units;
using MediatR;

namespace ESS.Application.Properties.List
{
    public sealed class Handler : IRequestHandler<Query, IReadOnlyList<PropertyListItemDto>>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IUnitRepository _unitRepository;

        public Handler(IPropertyRepository propertyRepository, IUnitRepository unitRepository)
        {
            _propertyRepository = propertyRepository;
            _unitRepository = unitRepository;
        }

        public async Task<IReadOnlyList<PropertyListItemDto>> Handle(Query request, CancellationToken ct)
        {
            var properties = await _propertyRepository.ListAsync(ct);
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
