using FluentValidation.TestHelper;
using QubicaMultiplex.Booking.Api.Models;
using QubicaMultiplex.Booking.Api.Validators;

namespace QubicaMultiplex.Booking.Api.Tests.ValidatorTests;

[TestClass]
public class BookingRequestValidatorTests
{
    private readonly BookingRequestValidator _sut = new BookingRequestValidator();

    [TestMethod]
    public void Should_NotHaveValidationErrors_When_RequestIsValid()
    {
        var bookingRequest = BookingRequest(new List<ShowBookingRequest> { ShowBookingRequest() });

        var actual = _sut.TestValidate(bookingRequest);

        actual.ShouldNotHaveAnyValidationErrors();
    }

    [TestMethod]
    public void Should_HaveValidationError_When_ShowsIsEmpty()
    {
        var bookingRequest = BookingRequest(new List<ShowBookingRequest>());

        var actual = _sut.TestValidate(bookingRequest);

        actual  
            .ShouldHaveValidationErrorFor(request => request.Shows)
            .WithErrorMessage("At least one show must be booked.");
    }

    [TestMethod]
    public void Should_HaveValidationError_When_CustomerIdIsNotPositive()
    {
        var bookingRequest = BookingRequest(new List<ShowBookingRequest> { ShowBookingRequest() }) with
        {
            Customer = Customer() with { Id = 0 }
        };

        var actual = _sut.TestValidate(bookingRequest);

        actual
            .ShouldHaveValidationErrorFor(request => request.Customer.Id)
            .WithErrorMessage("Customer id must be greater than 0.");
    }

    [TestMethod]
    public void Should_HaveValidationError_When_CustomerEmailIsNotValid()
    {
        var bookingRequest = BookingRequest(new List<ShowBookingRequest> { ShowBookingRequest() }) with
        {
            Customer = Customer() with { Email = "not-an-email" }
        };

        var actual = _sut.TestValidate(bookingRequest);

        actual
            .ShouldHaveValidationErrorFor(request => request.Customer.Email)
            .WithErrorMessage("Customer email is not valid.");
    }

    [TestMethod]
    public void Should_HaveValidationError_When_CustomerEmailIsEmpty()
    {
        var bookingRequest = BookingRequest(new List<ShowBookingRequest> { ShowBookingRequest() }) with
        {
            Customer = Customer() with { Email = "" }
        };

        var actual = _sut.TestValidate(bookingRequest);

        actual
            .ShouldHaveValidationErrorFor(request => request.Customer.Email)
            .WithErrorMessage("Customer email is required.");
    }

    [TestMethod]
    public void Should_HaveValidationError_When_ShowQuantityToBookIsLessThanOne()
    {
        var show = ShowBookingRequest() with
        {
            Seats = new SeatSelection { ChosenSeats = new List<Seat>(), QuantityToBook = 0 }
        };

        var bookingRequest = BookingRequest(new List<ShowBookingRequest> { show });

        var actual = _sut.TestValidate(bookingRequest);

        actual
            .ShouldHaveValidationErrorFor("Shows[0].Seats.QuantityToBook")
            .WithErrorMessage("Quantity to book must be at least 1.");
    }

    [TestMethod]
    public void Should_HaveValidationError_When_ShowIsRepeated()
    {
        var bookingRequest = BookingRequest(new List<ShowBookingRequest> { ShowBookingRequest(), ShowBookingRequest() });

        var actual = _sut.TestValidate(bookingRequest);

        actual
            .ShouldHaveValidationErrorFor(request => request.Shows)
            .WithErrorMessage("Each show can be booked only once per request.");
    }

    #region Utils

    private static BookingRequest BookingRequest(IReadOnlyCollection<ShowBookingRequest> shows)
        => new BookingRequest
        {
            Customer = Customer(),
            Shows = shows
        };

    private static Customer Customer()
        => new Customer
        {
            Id = 1,
            Email = "leonardo@example.com"
        };

    private static ShowBookingRequest ShowBookingRequest()
        => new ShowBookingRequest
        {
            ShowId = 13,
            Seats = new SeatSelection { ChosenSeats = new List<Seat>(), QuantityToBook = 2 }
        };

    #endregion
}
