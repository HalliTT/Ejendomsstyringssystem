using ESS.Api.Contracts.Properties;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESS.Api.Controllers
{
    [ApiController]
    [Route("api/properties")]
    public sealed class PropertyController : ControllerBase
    {
        private readonly ISender _sender;

        public PropertyController(ISender sender)
        {
            _sender = sender;
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ListProperties(CancellationToken ct)
        {
            var result = await _sender.Send(
                new Application.Properties.List.Query(),
            ct);

            return Ok(result);
        }

        [HttpGet("{propertyId:Guid}")]
        [Authorize]

        public async Task<IActionResult> GetProperty(Guid propertyId, CancellationToken ct)
        {
            var result = await _sender.Send(
                new Application.Properties.Get.Query(propertyId),
            ct);

            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateProperty([FromBody] CreatePropertyRequest request, CancellationToken ct)
        {
            var command = new Application.Properties.Create.Command(
                request.Name,
                request.Address,
                request.City,
                request.Country,
                request.Description);

            var result = await _sender.Send(command, ct);

            return Ok(result);
        }

        [HttpPut("{propertyId:Guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateProperty(Guid propertyId, [FromBody] UpdatePropertyRequest request, CancellationToken ct)
        {
            var command = new Application.Properties.Update.Command(
                propertyId,
                request.Name,
                request.Address,
                request.City,
                request.Country,
                request.Description);

            var result = await _sender.Send(command, ct);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{propertyId:Guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteProperty(Guid propertyId, CancellationToken ct)
        {
            var deleted = await _sender.Send(new Application.Properties.Delete.Command(propertyId), ct);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
