using MediatR;

namespace ESS.Application.Properties.List
{
    public sealed class Handler : IRequestHandler<Query, IReadOnlyList<PropertyDto>>
    {
        private readonly IPropertyRepository _propertyRepository;

        public Handler(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        public async Task<IReadOnlyList<PropertyDto>> Handle(Query request, CancellationToken ct)
        {
            var properties = await _propertyRepository.ListAsync(ct);
            return properties
                .Select(property => new PropertyDto(
                    property.Id,
                    property.Name,
                    property.Address))
                .ToList();

        }
    }
}
