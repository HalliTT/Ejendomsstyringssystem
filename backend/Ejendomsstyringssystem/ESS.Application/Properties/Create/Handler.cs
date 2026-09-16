using ESS.Application.Properties.Get;
using ESS.Application.Units.List;
using MediatR;

namespace ESS.Application.Properties.Create
{
    public sealed class Handler : IRequestHandler<Command, PropertyDto>
    {
        private readonly IPropertyRepository _propertyRepository;

        public Handler(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        public async Task<PropertyDto> Handle(Command command, CancellationToken ct)
        {
            var created = await _propertyRepository.CreateAsync(
                command.Name,
                command.Address,
                command.City,
                command.Country,
                command.Description,
                ct);


            return new PropertyDto(
                created!.Id,
                created.Name,
                created.Address,
                created.City,
                created.Country,
                created.Description,
                OccupiedUnits: 0,
                TotalUnits: 0,
                Units: new List<UnitListItemDto>()
                );
        }
    }
}
