using QubicaMultiplex.Customer.Api.Models;
using QubicaMultiplex.Customer.Api.Repositories.Interfaces;
using QubicaMultiplex.Customer.Api.Services.Interfaces;

namespace QubicaMultiplex.Customer.Api.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
        => _customerRepository = customerRepository;

    public async Task<CustomerDetails> GetCustomerAsync(int customerId, CancellationToken cancellationToken)
        => await _customerRepository.GetCustomerAsync(customerId, cancellationToken);

    public async Task<CustomerDetails> CreateCustomerAsync(CustomerRequest request, CancellationToken cancellationToken)
        => await _customerRepository.CreateCustomerAsync(request.Email, cancellationToken);
}
