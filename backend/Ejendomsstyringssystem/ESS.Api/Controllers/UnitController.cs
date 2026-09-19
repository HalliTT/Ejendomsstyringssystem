using ESS.Api.Contracts.Units;
using ESS.Domain.Units;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<IActionResult> ListUnits(CancellationToken ct)
        {
            var result = await _sender.Send(
                new Application.Units.List.Query(),
            ct);

            return Ok(result);
        }

        [HttpGet("{unitId:Guid}")]
        [Authorize]
        public async Task<IActionResult> GetUnit(Guid unitId, CancellationToken ct)
        {
            var result = await _sender.Send(
                new Application.Units.Get.Query(unitId),
            ct);

            return Ok(result);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateUnit([FromBody] CreateUnitRequest request, CancellationToken ct)
        {
            if (!TryParseStatus(request.Status, out var status))
                return BadRequest($"Invalid status '{request.Status}'.");

            var command = new Application.Units.Create.Command(
                request.Name,
                request.Description,
                status,
                request.PropertyId);

            var result = await _sender.Send(command, ct);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("{unitId:Guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateUnit(Guid unitId, [FromBody] UpdateUnitRequest request, CancellationToken ct)
        {
            if (!TryParseStatus(request.Status, out var status))
                return BadRequest($"Invalid status '{request.Status}'.");

            var command = new Application.Units.Update.Command(
                unitId,
                request.Name,
                request.Description,
                status);

            var result = await _sender.Send(command, ct);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{unitId:Guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteUnit(Guid unitId, CancellationToken ct)
        {
            var deleted = await _sender.Send(new Application.Units.Delete.Command(unitId), ct);

            if (!deleted)
                return NotFound();

            return NoContent();
        }

        private static bool TryParseStatus(string value, out UnitStatus status) =>
            Enum.TryParse(value, ignoreCase: true, out status) && Enum.IsDefined(status);
    }
}
