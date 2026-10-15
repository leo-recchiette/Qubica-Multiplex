namespace QubicaMultiplex.Catalog.Api.Models;

public record Showtime
{
    public required int ShowId { get; init; }
    public required DateTime StartsAt { get; init; }
    public required string Hall { get; init; }
}
