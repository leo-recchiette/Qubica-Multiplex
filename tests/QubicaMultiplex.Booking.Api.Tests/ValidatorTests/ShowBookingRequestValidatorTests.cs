using FluentValidation.TestHelper;
using QubicaMultiplex.Booking.Api.Models;
using QubicaMultiplex.Booking.Api.Validators;

namespace QubicaMultiplex.Booking.Api.Tests.ValidatorTests;

[TestClass]
public class ShowBookingRequestValidatorTests
{
    private readonly ShowBookingRequestValidator _sut = new ShowBookingRequestValidator();

    [TestMethod]
    public void Should_NotHaveValidationErrors_When_ShowIsValid()
    {
        var show = ShowBookingRequest();

        var actual = _sut.TestValidate(show);

        actual.ShouldNotHaveAnyValidationErrors();
    }

    [TestMethod]
    public void Should_HaveValidationError_When_ShowIdIsNotPositive()
    {
        var show = ShowBookingRequest() with { ShowId = 0 };

        var actual = _sut.TestValidate(show);

        actual
            .ShouldHaveValidationErrorFor(show => show.ShowId)
            .WithErrorMessage("Show id must be greater than 0.");
    }

    #region Utils

    private static ShowBookingRequest ShowBookingRequest()
        => new ShowBookingRequest
        {
            ShowId = 13,
            Seats = new SeatSelection { ChosenSeats = new List<Seat>(), QuantityToBook = 2 }
        };

    #endregion
}
