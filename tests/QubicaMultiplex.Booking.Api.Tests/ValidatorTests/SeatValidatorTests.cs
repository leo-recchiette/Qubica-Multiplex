using FluentValidation.TestHelper;
using QubicaMultiplex.Booking.Api.Models;
using QubicaMultiplex.Booking.Api.Validators;

namespace QubicaMultiplex.Booking.Api.Tests.ValidatorTests;

[TestClass]
public class SeatValidatorTests
{
    private readonly SeatValidator _sut = new SeatValidator();

    [TestMethod]
    public void Should_NotHaveValidationErrors_When_SeatIsValid()
    {
        var seat = Seat();

        var actual = _sut.TestValidate(seat);

        actual.ShouldNotHaveAnyValidationErrors();
    }

    [TestMethod]
    public void Should_HaveValidationError_When_RowIsMoreThanOneLetter()
    {
        var seat = Seat() with { Row = "FF" };

        var actual = _sut.TestValidate(seat);

        actual
            .ShouldHaveValidationErrorFor(seat => seat.Row)
            .WithErrorMessage("Seat row must be a single letter from A to Z.");
    }

    [TestMethod]
    public void Should_HaveValidationError_When_RowIsLowercase()
    {
        var seat = Seat() with { Row = "f" };

        var actual = _sut.TestValidate(seat);

        actual
            .ShouldHaveValidationErrorFor(seat => seat.Row)
            .WithErrorMessage("Seat row must be a single letter from A to Z.");
    }

    [TestMethod]
    public void Should_HaveValidationError_When_NumberIsNotPositive()
    {
        var seat = Seat() with { Number = 0 };

        var actual = _sut.TestValidate(seat);

        actual
            .ShouldHaveValidationErrorFor(seat => seat.Number)
            .WithErrorMessage("Seat number must be greater than 0.");
    }

    #region Utils

    private static Seat Seat()
        => new Seat
        {
            Row = "F",
            Number = 12
        };

    #endregion
}
