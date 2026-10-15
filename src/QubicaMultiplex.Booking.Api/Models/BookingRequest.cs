namespace QubicaMultiplex.Booking.Api.Models;

public record BookingRequest
{
    public required Customer Customer { get; init; }
    public required List<ShowBookingRequest> Shows { get; init; }
}
