namespace QubicaMultiplex.Booking.Api.Models;

public record BookingRequest
{
    public required int CustomerId { get; init; }
    public required IReadOnlyCollection<ShowBookingRequest> Shows { get; init; }
}
