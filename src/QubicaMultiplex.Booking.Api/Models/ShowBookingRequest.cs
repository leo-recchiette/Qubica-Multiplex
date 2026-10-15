namespace QubicaMultiplex.Booking.Api.Models;

public record ShowBookingRequest
{
    public required int ShowId { get; init; }
    public required SeatSelection Seats { get; init; }
}
