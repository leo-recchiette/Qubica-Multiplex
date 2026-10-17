using Microsoft.AspNetCore.Mvc;
using QubicaMultiplex.Booking.Api.Exceptions;
using QubicaMultiplex.Booking.Api.Models;
using QubicaMultiplex.Booking.Api.Services.Interfaces;

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

    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<BookingDetails>>> GetCustomerBookingsAsync(int customerId, CancellationToken cancellationToken)
    {
        var bookings = await _bookingService.GetCustomerBookingsAsync(customerId, cancellationToken);

        return bookings.Any()
            ? Ok(bookings)
            : NotFound();
    }

    [HttpPost("book")]
    public async Task<ActionResult<BookingDetails>> CreateBookingAsync(BookingRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await _bookingService.CreateBookingAsync(request, cancellationToken);
        }
        catch (ShowNotFoundException exception)
        {
            return Problem(detail: exception.Message, statusCode: StatusCodes.Status404NotFound);
        }
        catch (NotEnoughSeatsException exception)
        {
            return Problem(detail: exception.Message, statusCode: StatusCodes.Status409Conflict);
        }
        catch (SeatAlreadyBookedException exception)
        {
            return Problem(detail: exception.Message, statusCode: StatusCodes.Status409Conflict);
        }
    }

    [HttpDelete("remove/{bookingId}")]
    public async Task<IActionResult> DeleteBookingAsync(int bookingId, CancellationToken cancellationToken)
    {
        var deleted = await _bookingService.DeleteBookingAsync(bookingId, cancellationToken);

        return deleted
            ? NoContent()
            : NotFound();
    }
}
