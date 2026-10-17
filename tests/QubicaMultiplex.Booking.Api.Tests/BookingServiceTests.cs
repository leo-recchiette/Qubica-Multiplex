using AwesomeAssertions;
using AwesomeAssertions.Extensions;
using NSubstitute;
using QubicaMultiplex.Booking.Api.Models;
using QubicaMultiplex.Booking.Api.Repositories.Interfaces;
using QubicaMultiplex.Booking.Api.Services;
using QubicaMultiplex.Booking.Api.Services.Interfaces;

namespace QubicaMultiplex.Booking.Api.Tests;

[TestClass]
public class BookingServiceTests
{
    private readonly ISeatService _seatService = Substitute.For<ISeatService>();
    private readonly IBookingRepository _bookingRepository = Substitute.For<IBookingRepository>();
    private readonly BookingService _sut;

    public BookingServiceTests()
        => _sut = new BookingService(_seatService, _bookingRepository);

    [TestMethod]
    public async Task Should_ReturnBookingDetails_When_BookingIsCreated()
    {
        var bookingRequest = BookingRequest();

        _seatService
            .AssignSeatsAsync(bookingRequest.Shows, Arg.Any<CancellationToken>())
            .Returns(BookedShows());

        _bookingRepository
            .CreateBookingAsync(bookingRequest.Customer, Arg.Any<IEnumerable<BookedShow>>(), Arg.Any<CancellationToken>())
            .Returns(BookingEntity());

        var actual = await _sut.CreateBookingAsync(bookingRequest, CancellationToken.None);

        var expected = BookingDetails();

        actual.Should().BeEquivalentTo(expected);
    }

    #region Utils

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

    private static IReadOnlyCollection<BookedShow> BookedShows()
        => new List<BookedShow>
        {
            new BookedShow { ShowId = 13, Seats = new List<Seat> { Seat("F", 12), Seat("F", 13) } },
            new BookedShow { ShowId = 7, Seats = new List<Seat> { Seat("A", 1) } }
        };

    private static BookingEntity BookingEntity()
        => new BookingEntity
        {
            Id = 1,
            CreatedAt = 16.October(2026).At(22, 28)
        };

    private static BookingDetails BookingDetails()
        => new BookingDetails
        {
            Id = 1,
            CreatedAt = 16.October(2026).At(22, 28),
            Shows = BookedShows()
        };

    #endregion
}
