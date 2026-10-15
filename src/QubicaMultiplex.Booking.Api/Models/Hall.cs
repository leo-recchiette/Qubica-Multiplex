namespace QubicaMultiplex.Booking.Api.Models;

public record Hall
{
    public required string Name { get; init; }
    public required string LastRow { get; init; }
    public required int SeatsPerRow { get; init; }
}
