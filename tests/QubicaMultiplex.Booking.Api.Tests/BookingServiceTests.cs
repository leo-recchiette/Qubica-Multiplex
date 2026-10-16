using System.Net;
using System.Text.Json;
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

    [TestMethod]
    public async Task Should_ReturnBookingDetails_When_BookingIsCreated()
    {
        var bookingRequest = BookingRequest();

        _bookingRepository
            .CreateBookingAsync(bookingRequest, Arg.Any<CancellationToken>())
            .Returns(BookingEntity());

        var actual = await _sut.CreateBookingAsync(bookingRequest, CancellationToken.None);

        var expected = BookingDetails();

        Assert.AreEqual(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
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

    private static BookingRequest BookingRequest()
        => new BookingRequest
        {
            Customer = new Customer { Id = 1, Email = "leonardo@example.com" },
            Shows = new List<ShowBookingRequest>
            {
                new ShowBookingRequest
                {
                    ShowId = 13,
                    Seats = new SeatSelection { ChosenSeats = new List<Seat> { Seat("F", 12), Seat("F", 13) }, QuantityToBook = 2 }
                },
                new ShowBookingRequest
                {
                    ShowId = 7,
                    Seats = new SeatSelection { ChosenSeats = new List<Seat> { Seat("A", 1) } }
                }
            }
        };

    private static BookingEntity BookingEntity()
        => new BookingEntity
        {
            Id = 1,
            CreatedAt = new DateTime(2026, 10, 16, 22, 28, 0)
        };

    private static BookingDetails BookingDetails()
        => new BookingDetails
        {
            Id = 1,
            CreatedAt = new DateTime(2026, 10, 16, 22, 28, 0),
            Shows = new List<BookedShow>
            {
                new BookedShow { ShowId = 13, Seats = new List<Seat> { Seat("F", 12), Seat("F", 13) } },
                new BookedShow { ShowId = 7, Seats = new List<Seat> { Seat("A", 1) } }
            }
        };

    #endregion
}
