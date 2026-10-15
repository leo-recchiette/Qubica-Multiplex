namespace QubicaMultiplex.Catalog.Api.Models;

public record ShowtimeDetailsEntity
{
    public required int ShowId { get; init; }
    public required DateTime StartsAt { get; init; }
    public required string HallName { get; init; }
    public required string LastRow { get; init; }
    public required int SeatsPerRow { get; init; }
}
