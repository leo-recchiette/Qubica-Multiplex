using FluentValidation;
using QubicaMultiplex.Customer.Api.Models;
using QubicaMultiplex.Customer.Api.Repositories;
using QubicaMultiplex.Customer.Api.Repositories.Interfaces;
using QubicaMultiplex.Customer.Api.Services;
using QubicaMultiplex.Customer.Api.Services.Interfaces;
using QubicaMultiplex.Customer.Api.Validators;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace QubicaMultiplex.Customer.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomerServices(this IServiceCollection services)
    {
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        services.AddScoped<IValidator<CustomerRequest>, CustomerRequestValidator>();
        services.AddFluentValidationAutoValidation();

        return services;
    }
}
