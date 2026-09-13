using MediatR;

namespace ESS.Application.Properties.List
{
    public sealed class Handler : IRequestHandler<Query, IReadOnlyList<PropertyListItemDto>>
    {
        private readonly IPropertyRepository _propertyRepository;

        public Handler(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        public async Task<IReadOnlyList<PropertyListItemDto>> Handle(Query request, CancellationToken ct)
        {
            var properties = await _propertyRepository.ListAsync(ct);
            return properties
                .Select(property => new PropertyListItemDto(
                    property.Id,
                    property.Name,
                    property.Address))
                .ToList();

        }
    }
}
