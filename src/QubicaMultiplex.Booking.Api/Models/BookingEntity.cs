namespace QubicaMultiplex.Booking.Api.Models;

public record BookingEntity
{
    public required int Id { get; init; }
    public required DateTime CreatedAt { get; init; }
}
