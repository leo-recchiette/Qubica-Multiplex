using QubicaMultiplex.Customer.Api.Models;

namespace QubicaMultiplex.Customer.Api.Services.Interfaces;

public interface ICustomerService
{
    Task<CustomerDetails> GetCustomerAsync(int customerId, CancellationToken cancellationToken);

    Task<CustomerDetails> CreateCustomerAsync(CustomerRequest request, CancellationToken cancellationToken);
}
