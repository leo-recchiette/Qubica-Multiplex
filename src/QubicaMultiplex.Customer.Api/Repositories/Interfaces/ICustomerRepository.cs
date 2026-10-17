using QubicaMultiplex.Customer.Api.Models;

namespace QubicaMultiplex.Customer.Api.Repositories.Interfaces;

public interface ICustomerRepository
{
    Task<CustomerDetails> GetCustomerAsync(int customerId, CancellationToken cancellationToken);

    Task<CustomerDetails> CreateCustomerAsync(string email, CancellationToken cancellationToken);
}
