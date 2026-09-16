using ESS.Application.Units.Get;
using ESS.Domain.Units;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace ESS.Application.Units.Create
{
    public sealed class Handler : IRequestHandler<Command, UnitDto>
    {
        private readonly IUnitRepository _unitRepository;

        public Handler(IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
        }

        public async Task<UnitDto> Handle(Command command, CancellationToken ct) 
        {
            if (!Enum.TryParse<UnitStatus>(
              command.Status,
              ignoreCase: true,
              out var status))
            {
                throw new Exception();
            }


            var created = await _unitRepository.CreateAsync(
                command.Name,
                command.Description,
                status,
                command.PropertyId,
                ct);

            return new UnitDto(
                created!.Id,
                created.PropertyId,
                created.Name ?? "",
                created.Description ?? "",
                created.Status
                );
        }
    }
}
