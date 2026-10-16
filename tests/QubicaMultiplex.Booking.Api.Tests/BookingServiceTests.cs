using System.Net;
using NSubstitute;
using QubicaMultiplex.Booking.Api.Clients;
using QubicaMultiplex.Booking.Api.Models;
using QubicaMultiplex.Booking.Api.Repositories;
using QubicaMultiplex.Booking.Api.Services;
using Refit;

namespace QubicaMultiplex.Booking.Api.Tests;

[TestClass]
public class BookingServiceTests
{
    private readonly ICatalogClient _catalogClient = Substitute.For<ICatalogClient>();
    private readonly IBookingRepository _bookingRepository = Substitute.For<IBookingRepository>();
    private readonly BookingService _sut;

    public BookingServiceTests()
        => _sut = new BookingService(_catalogClient, _bookingRepository);

    [TestMethod]
    public async Task Should_ReturnNull_When_ShowtimeDoesNotExist()
    {
        _catalogClient
            .GetShowtimeDetailsAsync(999, Arg.Any<CancellationToken>())
            .Returns(ApiResponse(HttpStatusCode.NotFound));

        var actual = await _sut.GetAvailableSeatsAsync(999, CancellationToken.None);

        Assert.IsNull(actual);
    }

    [TestMethod]
    public async Task Should_ReturnAvailableSeats_When_ShowtimeExists()
    {
        _catalogClient
            .GetShowtimeDetailsAsync(13, Arg.Any<CancellationToken>())
            .Returns(ApiResponse(HttpStatusCode.OK, ShowtimeDetails()));

        _bookingRepository
            .GetBookedSeatsAsync(13, Arg.Any<CancellationToken>())
            .Returns(new List<Seat> { Seat("B", 2) });

        var actual = await _sut.GetAvailableSeatsAsync(13, CancellationToken.None);

        var expected = new List<Seat>
        {
            Seat("A", 1), Seat("A", 2), Seat("A", 3),
            Seat("B", 1), Seat("B", 3)
        };

        CollectionAssert.AreEqual(expected, actual!.ToList());
    }

    #region Utils

    private static IApiResponse<ShowtimeDetails> ApiResponse(HttpStatusCode statusCode, ShowtimeDetails? content = null)
        => new ApiResponse<ShowtimeDetails>(
            new HttpResponseMessage(statusCode) { RequestMessage = new HttpRequestMessage() },
            content,
            new RefitSettings());

    private static ShowtimeDetails ShowtimeDetails()
        => new ShowtimeDetails
        {
            ShowId = 13,
            StartsAt = new DateTime(2026, 10, 20, 21, 30, 0),
            Hall = Hall()
        };

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
