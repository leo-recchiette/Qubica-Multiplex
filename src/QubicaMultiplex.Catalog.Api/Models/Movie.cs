namespace QubicaMultiplex.Catalog.Api.Models;

public record Movie
{
    public required int Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Director { get; init; }
    public required int DurationMinutes { get; init; }
    public required DateTime ReleaseDate { get; init; }
}