using MediatR;

namespace ESS.Application.Units.Get
{
    public sealed class Handler : IRequestHandler<Query, UnitDto?>
    {
        private readonly IUnitRepository _unitRepository;

        public Handler(IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
        }

        public async Task<UnitDto?> Handle(Query request, CancellationToken ct)
        {
            var unit = await _unitRepository.GetByIdAsync(request.UnitId, ct);

            if (unit == null)
            {
                return null;
            }

            return new UnitDto(
                unit.Id,
                unit.PropertyId,
                unit.Name ?? "",
                unit.Description ?? "",
                unit.Status
             );
        }
    }
}
