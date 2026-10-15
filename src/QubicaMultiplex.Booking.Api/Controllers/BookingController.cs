using Microsoft.AspNetCore.Mvc;
using QubicaMultiplex.Booking.Api.Models;
using QubicaMultiplex.Booking.Api.Services;

namespace QubicaMultiplex.Booking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet("availability/{showId}")]
    public async Task<ActionResult<IEnumerable<Seat>>> GetAvailableSeatsAsync(int showId, CancellationToken cancellationToken)
    {
        var seats = await _bookingService.GetAvailableSeatsAsync(showId, cancellationToken);

        return seats is not null
            ? Ok(seats)
            : NotFound();
    }

    [HttpPost]
    public IActionResult CreateBooking(BookingRequest request)
    {
        // Placeholder
        return Ok();
    }
}
