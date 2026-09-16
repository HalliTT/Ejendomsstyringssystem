using ESS.Api.Contracts.Units;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ESS.Api.Controllers
{
    [ApiController]
    [Route("api/units")]
    public sealed class UnitController : ControllerBase
    {
        private readonly ISender _sender;

        public UnitController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> ListUnits(CancellationToken ct)
        {
            var result = await _sender.Send(
                new Application.Units.List.Query(),
            ct);

            return Ok(result);
        }

        [HttpGet("{unitId:Guid}")]
        public async Task<IActionResult> GetUnit(Guid unitId, CancellationToken ct)
        {
            var result = await _sender.Send(
                new Application.Units.Get.Query(unitId),
            ct);

            return Ok(result);
        }


        [HttpPost("unit")]
        public async Task<IActionResult> CreateUnit([FromBody] CreateUnitRequest request, CancellationToken ct)
        {
            var command = new Application.Units.Create.Command(
                request.Name,
                request.Description,
                request.Status,
                request.PropertyId);

            return Ok(command);
        }
    }
}
