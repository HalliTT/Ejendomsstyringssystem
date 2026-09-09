using MediatR;
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
        public async Task<IActionResult> ListProperties(CancellationToken ct)
        {
            var result = await _sender.Send(
                new Application.Properties.List.Query(),
            ct);

            return Ok(result);
        }
    }
}
