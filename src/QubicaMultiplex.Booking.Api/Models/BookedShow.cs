namespace QubicaMultiplex.Booking.Api.Models;

public record BookedShow
{
    public required int ShowId { get; init; }
    public required List<Seat> Seats { get; init; }
}
