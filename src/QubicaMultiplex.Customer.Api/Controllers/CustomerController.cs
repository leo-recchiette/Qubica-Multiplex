using Microsoft.AspNetCore.Mvc;
using QubicaMultiplex.Customer.Api.Exceptions;
using QubicaMultiplex.Customer.Api.Models;
using QubicaMultiplex.Customer.Api.Services.Interfaces;

namespace QubicaMultiplex.Customer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet("{customerId}")]
    public async Task<ActionResult<CustomerDetails>> GetCustomerAsync(int customerId, CancellationToken cancellationToken)
    {
        try
        {
            return await _customerService.GetCustomerAsync(customerId, cancellationToken);
        }
        catch (CustomerNotFoundException exception)
        {
            return Problem(detail: exception.Message, statusCode: StatusCodes.Status404NotFound);
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<CustomerDetails>> CreateCustomerAsync(CustomerRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await _customerService.CreateCustomerAsync(request, cancellationToken);
        }
        catch (EmailAlreadyRegisteredException exception)
        {
            return Problem(detail: exception.Message, statusCode: StatusCodes.Status409Conflict);
        }
    }
}
