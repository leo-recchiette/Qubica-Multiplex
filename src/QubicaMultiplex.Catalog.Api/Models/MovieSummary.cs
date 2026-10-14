namespace QubicaMultiplex.Catalog.Api.Models;

public record MovieSummary
{
    public required int Id { get; init; }
    public required string Title { get; init; }
}
