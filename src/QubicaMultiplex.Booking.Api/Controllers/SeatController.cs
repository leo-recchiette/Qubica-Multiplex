using Microsoft.AspNetCore.Mvc;
using QubicaMultiplex.Booking.Api.Models;
using QubicaMultiplex.Booking.Api.Services.Interfaces;

namespace QubicaMultiplex.Booking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeatController : ControllerBase
{
    private readonly ISeatService _seatService;

    public SeatController(ISeatService seatService)
    {
        _seatService = seatService;
    }

    [HttpGet("availability/{showId}")]
    public async Task<ActionResult<IEnumerable<Seat>>> GetAvailableSeatsAsync(int showId, CancellationToken cancellationToken)
    {
        var seats = await _seatService.GetAvailableSeatsAsync(showId, cancellationToken);

        return seats is not null
            ? Ok(seats)
            : NotFound();
    }
}
