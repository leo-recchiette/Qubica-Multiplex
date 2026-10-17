using QubicaMultiplex.Booking.Api.Models;
using Refit;

namespace QubicaMultiplex.Booking.Api.Clients;

public interface ICustomerClient
{
    [Get("/api/Customer/{customerId}")]
    Task<IApiResponse<Customer>> GetCustomerAsync(int customerId, CancellationToken cancellationToken);
}
