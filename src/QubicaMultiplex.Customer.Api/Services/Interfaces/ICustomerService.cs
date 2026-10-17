using QubicaMultiplex.Customer.Api.Models;

namespace QubicaMultiplex.Customer.Api.Services.Interfaces;

public interface ICustomerService
{
    Task<CustomerDetails> CreateCustomerAsync(CustomerRequest request, CancellationToken cancellationToken);
}
