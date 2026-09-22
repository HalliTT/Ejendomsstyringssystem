using ESS.Api.Contracts.Rentals;
using ESS.Domain.Rentals;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESS.Api.Controllers
{
    [ApiController]
    [Route("api/rental-options")]
    public sealed class RentalOptionController : ControllerBase
    {
        private readonly ISender _sender;

        public RentalOptionController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateRentalOption([FromBody] CreateRentalOptionRequest request, CancellationToken ct)
        {
            if (!TryParseStatus(request.Status, out var status))
                return BadRequest($"Invalid status '{request.Status}'.");

            var command = new Application.Rentals.Create.Command(
                request.Name,
                request.MonthlyRent,
                status,
                request.PropertyId,
                request.UnitIds ?? new List<Guid>());

            var result = await _sender.Send(command, ct);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("{rentalOptionId:Guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateRentalOption(Guid rentalOptionId, [FromBody] UpdateRentalOptionRequest request, CancellationToken ct)
        {
            if (!TryParseStatus(request.Status, out var status))
                return BadRequest($"Invalid status '{request.Status}'.");

            var command = new Application.Rentals.Update.Command(
                rentalOptionId,
                request.Name,
                request.MonthlyRent,
                status,
                request.UnitIds ?? new List<Guid>());

            var result = await _sender.Send(command, ct);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{rentalOptionId:Guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteRentalOption(Guid rentalOptionId, CancellationToken ct)
        {
            var deleted = await _sender.Send(new Application.Rentals.Delete.Command(rentalOptionId), ct);

            if (!deleted)
                return NotFound();

            return NoContent();
        }

        private static bool TryParseStatus(string value, out RentalOptionStatus status) =>
            Enum.TryParse(value, ignoreCase: true, out status) && Enum.IsDefined(status);
    }
}
