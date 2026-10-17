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
            CustomerId = 0
        };

        var actual = _sut.TestValidate(bookingRequest);

        actual
            .ShouldHaveValidationErrorFor(request => request.CustomerId)
            .WithErrorMessage("Customer id must be greater than 0.");
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
            CustomerId = 1,
            Shows = shows
        };

    private static ShowBookingRequest ShowBookingRequest()
        => new ShowBookingRequest
        {
            ShowId = 13,
            Seats = new SeatSelection { ChosenSeats = new List<Seat>(), QuantityToBook = 2 }
        };

    #endregion
}
