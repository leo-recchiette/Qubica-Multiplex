namespace QubicaMultiplex.Booking.Api.Models;

public record BookingRequest
{
    public required Customer Customer { get; init; }
    public required IReadOnlyCollection<ShowBookingRequest> Shows { get; init; }
}
