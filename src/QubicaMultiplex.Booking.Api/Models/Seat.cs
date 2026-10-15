namespace QubicaMultiplex.Booking.Api.Models;

public record Seat
{
    public required string Row { get; init; }
    public required int Number { get; init; }
}
