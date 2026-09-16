using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESS.Application.Units.List
{
    public sealed class Handler : IRequestHandler<Query, IReadOnlyList<UnitListItemDto>>
    {
        private readonly IUnitRepository _unitRepository;

        public Handler(IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
        }

        public async Task<IReadOnlyList<UnitListItemDto>> Handle(Query request, CancellationToken ct)
        {
            var units = await _unitRepository.ListAsync(ct);
            return units
                .Select(unit => new UnitListItemDto(
                    unit.Id,
                    unit.Name,
                    unit.Description,
                    unit.Status
                    ))
                .ToList();
        }
    }
}
