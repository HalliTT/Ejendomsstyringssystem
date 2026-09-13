using ESS.Application.Units;
using ESS.Application.Units.List;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESS.Application.Properties.Get
{
    public sealed class Handler : IRequestHandler<Query, PropertyDto?>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IUnitRepository _unitRepository;

        public Handler(IPropertyRepository propertyRepository, IUnitRepository unitRepository)
        {
            _propertyRepository = propertyRepository;
            _unitRepository = unitRepository;
        }

        public async Task<PropertyDto?> Handle(Query request, CancellationToken ct)
        {
            var property = await _propertyRepository.GetByIdAsync(request.PropertyId, ct);

            if (property is null)
            {
                return null;
            }

            var units = await  _unitRepository.ListByPropertyIdAsync(request.PropertyId, ct);

            return new PropertyDto(
                property.Id,
                property.Name ?? "",
                property.Address ?? "",
                property.City ?? "",
                property.Country ?? "",
                property.Description ?? "",
                units.Select(u => new UnitListItemDto(
                    u.Id,
                    u.Name ?? "",
                    u.Description ?? ""))
                .ToList()
            );
        }
    }
}
