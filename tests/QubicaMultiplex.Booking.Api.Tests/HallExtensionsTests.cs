using QubicaMultiplex.Booking.Api.Extensions;
using QubicaMultiplex.Booking.Api.Models;

namespace QubicaMultiplex.Booking.Api.Tests;

[TestClass]
public class HallExtensionsTests
{
    [TestMethod]
    public void Should_ReturnAllSeats_When_NoSeatsAreBooked()
    {
        var actual = Hall().GetAvailableSeats([]);

        var expected = new List<Seat>
        {
            Seat("A", 1), Seat("A", 2), Seat("A", 3),
            Seat("B", 1), Seat("B", 2), Seat("B", 3)
        };

        CollectionAssert.AreEqual(expected, actual.ToList());
    }

    [TestMethod]
    public void Should_ExcludeBookedSeats_When_SomeSeatsAreBooked()
    {
        var bookedSeats = new List<Seat> { Seat("B", 2) };

        var actual = Hall().GetAvailableSeats(bookedSeats);

        var expected = new List<Seat>
        {
            Seat("A", 1), Seat("A", 2), Seat("A", 3),
            Seat("B", 1), Seat("B", 3)
        };

        CollectionAssert.AreEqual(expected, actual.ToList());
    }

    [TestMethod]
    public void Should_ReturnNoSeats_When_AllSeatsAreBooked()
    {
        var bookedSeats = new List<Seat>
        {
            Seat("A", 1), Seat("A", 2), Seat("A", 3),
            Seat("B", 1), Seat("B", 2), Seat("B", 3)
        };

        var actual = Hall().GetAvailableSeats(bookedSeats);

        Assert.IsEmpty(actual);
    }

    #region Utils

    private static Hall Hall()
        => new Hall
        {
            Name = "Hall 1",
            LastRow = "B",
            SeatsPerRow = 3
        };

    private static Seat Seat(string row, int number)
        => new Seat
        {
            Row = row,
            Number = number
        };

    #endregion
}
