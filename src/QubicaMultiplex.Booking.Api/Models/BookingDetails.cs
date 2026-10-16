namespace QubicaMultiplex.Booking.Api.Models;

public record BookingDetails
{
    public required int Id { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required List<BookedShow> Shows { get; init; }
}
