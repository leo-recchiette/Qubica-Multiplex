namespace QubicaMultiplex.Booking.Api.Models;

public record SeatSelection
{
    public required IReadOnlyCollection<Seat> ChosenSeats { get; init; }
    public int QuantityToBook { get; init; } = 1;
}
