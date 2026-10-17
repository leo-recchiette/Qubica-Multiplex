namespace QubicaMultiplex.Customer.Api.Models;

public record CustomerDetails
{
    public required int Id { get; init; }
    public required string Email { get; init; }
    public required DateTime CreatedAt { get; init; }
}
