namespace QubicaMultiplex.Booking.Api.Models;

public record BookedShow
{
    public required int ShowId { get; init; }
    public required IReadOnlyCollection<Seat> Seats { get; init; }
}
