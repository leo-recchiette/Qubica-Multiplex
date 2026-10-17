using FluentValidation;
using QubicaMultiplex.Customer.Api.Models;

namespace QubicaMultiplex.Customer.Api.Validators;

public class CustomerRequestValidator : AbstractValidator<CustomerRequest>
{
    private const string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

    public CustomerRequestValidator()
    {
        RuleFor(request => request.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email is required.")
            .Matches(EmailPattern).WithMessage("Email must be in the correct format.");
    }
}
