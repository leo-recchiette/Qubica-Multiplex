namespace QubicaMultiplex.Booking.Api.Models;

public record Customer
{
    public required int Id { get; init; }
    public required string Email { get; init; }
}
