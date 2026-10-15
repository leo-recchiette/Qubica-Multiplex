namespace QubicaMultiplex.Booking.Api.Models;

public record SeatSelection
{
    public required List<int> ChosenSeats { get; init; }
    public int QuantityToBook { get; init; } = 1;
}
