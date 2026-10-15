using Microsoft.AspNetCore.Mvc;
using QubicaMultiplex.Booking.Api.Models;

namespace QubicaMultiplex.Booking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateBooking(BookingRequest request)
    {
        // Placeholder
        return Ok();
    }
}
