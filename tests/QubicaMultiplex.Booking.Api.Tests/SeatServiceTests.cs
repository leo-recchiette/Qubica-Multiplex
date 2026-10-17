using System.Net;
using AwesomeAssertions;
using AwesomeAssertions.Extensions;
using NSubstitute;
using QubicaMultiplex.Booking.Api.Clients;
using QubicaMultiplex.Booking.Api.Exceptions;
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
    public async Task Should_ThrowShowNotFoundException_When_ShowtimeDoesNotExist()
    {
        _catalogClient
            .GetShowtimeDetailsAsync(999, Arg.Any<CancellationToken>())
            .Returns(ApiResponse(HttpStatusCode.NotFound));

        var actual = () => _sut.GetAvailableSeatsAsync(999, CancellationToken.None);

        await actual.Should().ThrowAsync<ShowNotFoundException>();
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
        _catalogClient
            .GetShowtimeDetailsAsync(13, Arg.Any<CancellationToken>())
            .Returns(ApiResponse(HttpStatusCode.OK, ShowtimeDetails()));

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
    public async Task Should_ThrowShowNotFoundException_When_ChosenSeatsAreForAMissingShow()
    {
        _catalogClient
            .GetShowtimeDetailsAsync(999, Arg.Any<CancellationToken>())
            .Returns(ApiResponse(HttpStatusCode.NotFound));

        var shows = new List<ShowBookingRequest>
        {
            ShowBookingRequest(999, new List<Seat> { Seat("B", 2) }, 1)
        };

        var actual = () => _sut.AssignSeatsAsync(shows, CancellationToken.None);

        await actual.Should().ThrowAsync<ShowNotFoundException>();
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

    [TestMethod]
    public async Task Should_ThrowNotEnoughSeatsException_When_AvailableSeatsAreNotEnough()
    {
        _catalogClient
            .GetShowtimeDetailsAsync(13, Arg.Any<CancellationToken>())
            .Returns(ApiResponse(HttpStatusCode.OK, ShowtimeDetails()));

        _bookingRepository
            .GetBookedSeatsAsync(13, Arg.Any<CancellationToken>())
            .Returns(new List<Seat> { Seat("A", 1), Seat("A", 2), Seat("A", 3), Seat("B", 1) });

        var shows = new List<ShowBookingRequest>
        {
            ShowBookingRequest(13, new List<Seat>(), 5)
        };

        var actual = () => _sut.AssignSeatsAsync(shows, CancellationToken.None);

        await actual.Should().ThrowAsync<NotEnoughSeatsException>()
            .WithMessage("Show 13 has only 2 available seats, 5 requested.");
    }

    [TestMethod]
    public async Task Should_ThrowSeatNotAvailableException_When_ChosenSeatIsAlreadyBooked()
    {
        _catalogClient
            .GetShowtimeDetailsAsync(13, Arg.Any<CancellationToken>())
            .Returns(ApiResponse(HttpStatusCode.OK, ShowtimeDetails()));

        _bookingRepository
            .GetBookedSeatsAsync(13, Arg.Any<CancellationToken>())
            .Returns(new List<Seat> { Seat("B", 2) });

        var shows = new List<ShowBookingRequest>
        {
            ShowBookingRequest(13, new List<Seat> { Seat("B", 1), Seat("B", 2) }, 2)
        };

        var actual = () => _sut.AssignSeatsAsync(shows, CancellationToken.None);

        await actual.Should().ThrowAsync<SeatNotAvailableException>()
            .WithMessage("Seats B2 are not available for show 13.");
    }

    [TestMethod]
    public async Task Should_ThrowSeatNotAvailableException_When_ChosenSeatDoesNotExist()
    {
        _catalogClient
            .GetShowtimeDetailsAsync(13, Arg.Any<CancellationToken>())
            .Returns(ApiResponse(HttpStatusCode.OK, ShowtimeDetails()));

        var shows = new List<ShowBookingRequest>
        {
            ShowBookingRequest(13, new List<Seat> { Seat("Z", 99) }, 1)
        };

        var actual = () => _sut.AssignSeatsAsync(shows, CancellationToken.None);

        await actual.Should().ThrowAsync<SeatNotAvailableException>()
            .WithMessage("Seats Z99 are not available for show 13.");
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
