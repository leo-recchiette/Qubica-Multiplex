namespace QubicaMultiplex.Customer.Api.Models;

public record CustomerRequest
{
    public required string Email { get; init; }
}
