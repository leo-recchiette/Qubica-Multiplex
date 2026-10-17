using FluentValidation.TestHelper;
using QubicaMultiplex.Booking.Api.Models;
using QubicaMultiplex.Booking.Api.Validators;

namespace QubicaMultiplex.Booking.Api.Tests.ValidatorTests;

[TestClass]
public class SeatSelectionValidatorTests
{
    private readonly SeatSelectionValidator _sut = new SeatSelectionValidator();

    [TestMethod]
    public void Should_NotHaveValidationErrors_When_SelectionIsValid()
    {
        var seatSelection = SeatSelection();

        var actual = _sut.TestValidate(seatSelection);

        actual.ShouldNotHaveAnyValidationErrors();
    }

    [TestMethod]
    public void Should_HaveValidationError_When_QuantityToBookIsLessThanOne()
    {
        var seatSelection = SeatSelection() with { QuantityToBook = 0 };

        var actual = _sut.TestValidate(seatSelection);

        actual
            .ShouldHaveValidationErrorFor(selection => selection.QuantityToBook)
            .WithErrorMessage("Quantity to book must be at least 1.");
    }

    [TestMethod]
    public void Should_NotHaveValidationErrors_When_ChosenSeatsMatchQuantityToBook()
    {
        var seatSelection = SeatSelection() with
        {
            ChosenSeats = new List<Seat> { Seat("F", 12), Seat("F", 13) }
        };

        var actual = _sut.TestValidate(seatSelection);

        actual.ShouldNotHaveAnyValidationErrors();
    }

    [TestMethod]
    public void Should_HaveValidationError_When_ChosenSeatsDoNotMatchQuantityToBook()
    {
        var seatSelection = SeatSelection() with
        {
            ChosenSeats = new List<Seat> { Seat("F", 12), Seat("F", 13) },
            QuantityToBook = 3
        };

        var actual = _sut.TestValidate(seatSelection);

        actual
            .ShouldHaveValidationErrorFor(selection => selection.ChosenSeats)
            .WithErrorMessage("The number of chosen seats must match the quantity to book.");
    }

    [TestMethod]
    public void Should_HaveValidationError_When_ChosenSeatsContainDuplicates()
    {
        var seatSelection = SeatSelection() with
        {
            ChosenSeats = new List<Seat> { Seat("F", 12), Seat("F", 12) }
        };

        var actual = _sut.TestValidate(seatSelection);

        actual
            .ShouldHaveValidationErrorFor(selection => selection.ChosenSeats)
            .WithErrorMessage("Chosen seats must not contain duplicates.");
    }

    [TestMethod]
    public void Should_HaveValidationError_When_ChosenSeatIsNotValid()
    {
        var seatSelection = SeatSelection() with
        {
            ChosenSeats = new List<Seat> { Seat("F", 12), Seat("FF", 13) }
        };

        var actual = _sut.TestValidate(seatSelection);

        actual
            .ShouldHaveValidationErrorFor("ChosenSeats[1].Row")
            .WithErrorMessage("Seat row must be a single letter from A to Z.");
    }

    #region Utils

    private static SeatSelection SeatSelection()
        => new SeatSelection
        {
            ChosenSeats = new List<Seat>(),
            QuantityToBook = 2
        };

    private static Seat Seat(string row, int number)
        => new Seat
        {
            Row = row,
            Number = number
        };

    #endregion
}
