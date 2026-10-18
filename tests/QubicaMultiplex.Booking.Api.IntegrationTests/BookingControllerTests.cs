using System.Net;
using System.Net.Http.Json;
using AwesomeAssertions;
using QubicaMultiplex.Booking.Api.Models;

namespace QubicaMultiplex.Booking.Api.IntegrationTests;

[TestClass]
[UsesVerify]
public partial class BookingControllerTests
{
    private readonly BookingApiFixture _bookingApiFixture = new();
    private readonly HttpClient _httpClient;

    public BookingControllerTests()
        => _httpClient = _bookingApiFixture.CreateClient();

    [TestInitialize]
    public Task ResetDatabaseAsync()
        => _bookingApiFixture.ResetDatabaseAsync();

    [TestMethod]
    public async Task Should_BookChosenSeats_When_SeatsAreAvailable()
    {
        var response = await BookAsync(BookingRequest(ShowBookingRequest(1, 2, Seat("A", 1), Seat("A", 2))));

        var actual = await response.Content.ReadAsStringAsync();

        await VerifyJson(actual);
    }

    [TestMethod]
    public async Task Should_AssignFirstAvailableSeats_When_NoSeatIsChosen()
    {
        await BookAsync(BookingRequest(ShowBookingRequest(1, 1, Seat("A", 1))));

        var response = await BookAsync(BookingRequest(ShowBookingRequest(1, 2)));

        var actual = await response.Content.ReadAsStringAsync();

        await VerifyJson(actual);
    }

    [TestMethod]
    public async Task Should_ReturnConflict_When_SeatIsAlreadyBooked()
    {
        await BookAsync(BookingRequest(ShowBookingRequest(1, 1, Seat("A", 1))));

        var response = await BookAsync(BookingRequest(ShowBookingRequest(1, 1, Seat("A", 1))));

        var actual = await response.Content.ReadAsStringAsync();

        await VerifyJson(actual);
    }

    [TestMethod]
    public async Task Should_BookSeatOnlyOnce_When_RequestsArriveTogether()
    {
        var requests = Enumerable.Range(0, 5).Select(_ => BookAsync(BookingRequest(ShowBookingRequest(1, 1, Seat("A", 1)))));

        var actual = await Task.WhenAll(requests);

        var expected = new[] { HttpStatusCode.OK, HttpStatusCode.Conflict, HttpStatusCode.Conflict, HttpStatusCode.Conflict, HttpStatusCode.Conflict };

        actual.Select(response => response.StatusCode).Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public async Task Should_ReturnConflict_When_NotEnoughSeatsAreAvailable()
    {
        var response = await BookAsync(BookingRequest(ShowBookingRequest(1, 5)));

        var actual = await response.Content.ReadAsStringAsync();

        await VerifyJson(actual);
    }

    [TestMethod]
    public async Task Should_SaveNothing_When_OneShowCannotBeBooked()
    {
        await BookAsync(BookingRequest(ShowBookingRequest(1, 1, Seat("A", 1)), ShowBookingRequest(2, 5)));

        var actual = await _httpClient.GetAsync("api/Booking/customer/1");

        actual.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task Should_ReturnNotFound_When_CustomerDoesNotExist()
    {
        var response = await BookAsync(BookingRequest(ShowBookingRequest(1, 1)) with { CustomerId = 99 });

        var actual = await response.Content.ReadAsStringAsync();

        await VerifyJson(actual);
    }

    [TestMethod]
    public async Task Should_ReturnNotFound_When_ShowDoesNotExist()
    {
        var response = await BookAsync(BookingRequest(ShowBookingRequest(99, 1)));

        var actual = await response.Content.ReadAsStringAsync();

        await VerifyJson(actual);
    }

    [TestMethod]
    public async Task Should_ReturnBadRequest_When_RequestIsNotValid()
    {
        var response = await BookAsync(BookingRequest());

        var actual = await response.Content.ReadAsStringAsync();

        await VerifyJson(actual);
    }

    [TestMethod]
    public async Task Should_ReturnCustomerBookings_When_CustomerHasBookings()
    {
        await BookAsync(BookingRequest(ShowBookingRequest(1, 1, Seat("A", 1))));

        var actual = await _httpClient.GetStringAsync("api/Booking/customer/1");

        await VerifyJson(actual);
    }

    [TestMethod]
    public async Task Should_ReturnNotFound_When_CustomerHasNoBookings()
    {
        var actual = await _httpClient.GetAsync("api/Booking/customer/1");

        actual.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task Should_DeleteBooking_When_BookingExists()
    {
        await BookAsync(BookingRequest(ShowBookingRequest(1, 1, Seat("A", 1))));

        var actual = await _httpClient.DeleteAsync("api/Booking/remove/1");

        actual.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [TestMethod]
    public async Task Should_ReturnNotFound_When_BookingDoesNotExist()
    {
        var actual = await _httpClient.DeleteAsync("api/Booking/remove/99");

        actual.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task Should_KeepOtherShows_When_OneShowIsRemoved()
    {
        await BookAsync(BookingRequest(ShowBookingRequest(1, 1, Seat("A", 1)), ShowBookingRequest(2, 1, Seat("A", 1))));

        await _httpClient.DeleteAsync("api/Booking/remove/1/show/1");

        var actual = await _httpClient.GetStringAsync("api/Booking/customer/1");

        await VerifyJson(actual);
    }

    [TestMethod]
    public async Task Should_DeleteBooking_When_LastShowIsRemoved()
    {
        await BookAsync(BookingRequest(ShowBookingRequest(1, 1, Seat("A", 1))));

        await _httpClient.DeleteAsync("api/Booking/remove/1/show/1");

        var actual = await _httpClient.GetAsync("api/Booking/customer/1");

        actual.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #region Utils

    private Task<HttpResponseMessage> BookAsync(BookingRequest bookingRequest)
        => _httpClient.PostAsJsonAsync("api/Booking/book", bookingRequest);

    private static BookingRequest BookingRequest(params ShowBookingRequest[] shows)
        => new BookingRequest
        {
            CustomerId = 1,
            Shows = shows
        };

    private static ShowBookingRequest ShowBookingRequest(int showId, int quantityToBook, params Seat[] chosenSeats)
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
