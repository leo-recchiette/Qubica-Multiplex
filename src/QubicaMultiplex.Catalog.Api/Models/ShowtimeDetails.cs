namespace QubicaMultiplex.Catalog.Api.Models;

public record ShowtimeDetails
{
    public required int ShowId { get; init; }
    public required DateTime StartsAt { get; init; }
    public required Hall Hall { get; init; }
}
