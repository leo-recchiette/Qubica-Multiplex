using FluentValidation.TestHelper;
using QubicaMultiplex.Customer.Api.Models;
using QubicaMultiplex.Customer.Api.Validators;

namespace QubicaMultiplex.Customer.Api.Tests.ValidatorTests;

[TestClass]
public class CustomerRequestValidatorTests
{
    private readonly CustomerRequestValidator _sut = new CustomerRequestValidator();

    [TestMethod]
    public void Should_NotHaveValidationErrors_When_RequestIsValid()
    {
        var customerRequest = CustomerRequest();

        var actual = _sut.TestValidate(customerRequest);

        actual.ShouldNotHaveAnyValidationErrors();
    }

    [TestMethod]
    public void Should_HaveValidationError_When_EmailIsEmpty()
    {
        var customerRequest = CustomerRequest() with { Email = "" };

        var actual = _sut.TestValidate(customerRequest);

        actual
            .ShouldHaveValidationErrorFor(request => request.Email)
            .WithErrorMessage("Email is required.");
    }

    [TestMethod]
    public void Should_HaveValidationError_When_EmailIsNotValid()
    {
        var customerRequest = CustomerRequest() with { Email = "not-an-email" };

        var actual = _sut.TestValidate(customerRequest);

        actual
            .ShouldHaveValidationErrorFor(request => request.Email)
            .WithErrorMessage("Email must be in the correct format.");
    }

    [TestMethod]
    public void Should_HaveValidationError_When_EmailHasNoDomainExtension()
    {
        var customerRequest = CustomerRequest() with { Email = "user@example" };

        var actual = _sut.TestValidate(customerRequest);

        actual
            .ShouldHaveValidationErrorFor(request => request.Email)
            .WithErrorMessage("Email must be in the correct format.");
    }

    [TestMethod]
    public void Should_HaveValidationError_When_EmailContainsSpaces()
    {
        var customerRequest = CustomerRequest() with { Email = "user rossi@example.com" };

        var actual = _sut.TestValidate(customerRequest);

        actual
            .ShouldHaveValidationErrorFor(request => request.Email)
            .WithErrorMessage("Email must be in the correct format.");
    }

    #region Utils

    private static CustomerRequest CustomerRequest()
        => new CustomerRequest
        {
            Email = "user@example.com"
        };

    #endregion
}
