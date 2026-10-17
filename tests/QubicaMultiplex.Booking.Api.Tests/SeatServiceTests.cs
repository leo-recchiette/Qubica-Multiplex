using System.Net;
using AwesomeAssertions;
using AwesomeAssertions.Extensions;
using NSubstitute;
using QubicaMultiplex.Booking.Api.Clients;
using QubicaMultiplex.Booking.Api.Models;
using QubicaMultiplex.Booking.Api.Repositories.Interfaces;
using QubicaMultiplex.Booking.Api.Services;
using Refit;

namespace QubicaMultiplex.Booking.Api.Tests;

[TestClass]
public class SeatServiceTests
{
    private readonly ICatalogClient _catalogClient = Substitute.For<ICatalogClient>();
    private readonly IBookingRepository _bookingRepository = Substitute.For<IBookingRepository>();
    private readonly SeatService _sut;

    public SeatServiceTests()
        => _sut = new SeatService(_catalogClient, _bookingRepository);

    [TestMethod]
    public async Task Should_ReturnNull_When_ShowtimeDoesNotExist()
    {
        _catalogClient
            .GetShowtimeDetailsAsync(999, Arg.Any<CancellationToken>())
            .Returns(ApiResponse(HttpStatusCode.NotFound));

        var actual = await _sut.GetAvailableSeatsAsync(999, CancellationToken.None);

        actual.Should().BeNull();
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

        actual.Should().Equal(expected);
    }

    [TestMethod]
    public async Task Should_KeepChosenSeats_When_SeatsAreChosen()
    {
        var shows = new List<ShowBookingRequest>
        {
            ShowBookingRequest(13, new List<Seat> { Seat("B", 2), Seat("B", 3) }, 2)
        };

        var actual = await _sut.AssignSeatsAsync(shows, CancellationToken.None);

        var expected = new List<BookedShow>
        {
            new BookedShow { ShowId = 13, Seats = new List<Seat> { Seat("B", 2), Seat("B", 3) } }
        };

        actual.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public async Task Should_AssignFirstAvailableSeats_When_NoSeatsAreChosen()
    {
        _catalogClient
            .GetShowtimeDetailsAsync(13, Arg.Any<CancellationToken>())
            .Returns(ApiResponse(HttpStatusCode.OK, ShowtimeDetails()));

        _bookingRepository
            .GetBookedSeatsAsync(13, Arg.Any<CancellationToken>())
            .Returns(new List<Seat> { Seat("A", 1) });

        var shows = new List<ShowBookingRequest>
        {
            ShowBookingRequest(13, new List<Seat>(), 2)
        };

        var actual = await _sut.AssignSeatsAsync(shows, CancellationToken.None);

        var expected = new List<BookedShow>
        {
            new BookedShow { ShowId = 13, Seats = new List<Seat> { Seat("A", 2), Seat("A", 3) } }
        };

        actual.Should().BeEquivalentTo(expected);
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
            StartsAt = 20.October(2026).At(21, 30),
            Hall = Hall()
        };

    private static Hall Hall()
        => new Hall
        {
            Name = "Hall 1",
            LastRow = "B",
            SeatsPerRow = 3
        };

    private static ShowBookingRequest ShowBookingRequest(int showId, List<Seat> chosenSeats, int quantityToBook)
        => new ShowBookingRequest
        {
            ShowId = showId,
            Seats = new SeatSelection { ChosenSeats = chosenSeats, QuantityToBook = quantityToBook }
        };

    private static Seat Seat(string row, int number)
        => new Seat
        {
            Row = row,
            Number = number
        };

    #endregion
}
