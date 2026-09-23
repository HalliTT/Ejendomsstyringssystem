using ESS.Api.Contracts.Bookings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESS.Api.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    public sealed class BookingController : ControllerBase
    {
        private readonly ISender _sender;

        public BookingController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ListBookings(CancellationToken ct)
        {
            var result = await _sender.Send(new Application.Bookings.List.Query(), ct);
            return Ok(result);
        }

        [HttpGet("{bookingId:Guid}")]
        [Authorize]
        public async Task<IActionResult> GetBooking(Guid bookingId, CancellationToken ct)
        {
            var result = await _sender.Send(new Application.Bookings.Get.Query(bookingId), ct);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request, CancellationToken ct)
        {
            if (request.EndDate <= request.StartDate)
                return BadRequest("End date must be after start date.");

            var command = new Application.Bookings.Create.Command(
                request.RentalOptionId,
                request.TenantId,
                request.StartDate,
                request.EndDate);

            var result = await _sender.Send(command, ct);

            if (result.HasOverlap)
                return Conflict("This rental option is already booked for the selected dates.");

            if (result.NotFound || result.Booking is null)
                return NotFound();

            return Ok(result.Booking);
        }

        [HttpPut("{bookingId:Guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateBooking(Guid bookingId, [FromBody] UpdateBookingRequest request, CancellationToken ct)
        {
            if (request.EndDate <= request.StartDate)
                return BadRequest("End date must be after start date.");

            var command = new Application.Bookings.Update.Command(
                bookingId,
                request.RentalOptionId,
                request.TenantId,
                request.StartDate,
                request.EndDate);

            var result = await _sender.Send(command, ct);

            if (result.HasOverlap)
                return Conflict("This rental option is already booked for the selected dates.");

            if (result.NotFound || result.Booking is null)
                return NotFound();

            return Ok(result.Booking);
        }

        [HttpDelete("{bookingId:Guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteBooking(Guid bookingId, CancellationToken ct)
        {
            var deleted = await _sender.Send(new Application.Bookings.Delete.Command(bookingId), ct);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
