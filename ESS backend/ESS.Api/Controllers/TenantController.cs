using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESS.Api.Controllers
{
    [ApiController]
    [Route("api/tenants")]
    public sealed class TenantController : ControllerBase
    {
        private readonly ISender _sender;

        public TenantController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ListTenants(CancellationToken ct)
        {
            var result = await _sender.Send(new Application.Tenants.List.Query(), ct);
            return Ok(result);
        }
    }
}
