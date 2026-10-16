namespace QubicaMultiplex.Booking.Api.Models;

public record BookingDetailsEntity
{
    public required int BookingId { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required int ShowId { get; init; }
    public required string Row { get; init; }
    public required int Number { get; init; }
}
