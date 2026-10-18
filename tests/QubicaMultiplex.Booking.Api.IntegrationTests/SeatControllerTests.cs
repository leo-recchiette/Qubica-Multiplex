using System.Net.Http.Json;
using QubicaMultiplex.Booking.Api.Models;

namespace QubicaMultiplex.Booking.Api.IntegrationTests;

[TestClass]
[UsesVerify]
public partial class SeatControllerTests
{
    private readonly BookingApiFixture _bookingApiFixture = new();
    private readonly HttpClient _httpClient;

    public SeatControllerTests()
        => _httpClient = _bookingApiFixture.CreateClient();

    [TestInitialize]
    public Task ResetDatabaseAsync()
        => _bookingApiFixture.ResetDatabaseAsync();

    [TestMethod]
    public async Task Should_ExcludeBookedSeats_When_SomeSeatsAreBooked()
    {
        await _httpClient.PostAsJsonAsync("api/Booking/book", BookingRequest());

        var actual = await _httpClient.GetStringAsync("api/Seat/availability/1");

        await VerifyJson(actual);
    }

    [TestMethod]
    public async Task Should_ReturnNotFound_When_ShowDoesNotExist()
    {
        var response = await _httpClient.GetAsync("api/Seat/availability/99");

        var actual = await response.Content.ReadAsStringAsync();

        await VerifyJson(actual);
    }

    #region Utils

    private static BookingRequest BookingRequest()
        => new BookingRequest
        {
            CustomerId = 1,
            Shows = new List<ShowBookingRequest>
            {
                new ShowBookingRequest
                {
                    ShowId = 1,
                    Seats = new SeatSelection { ChosenSeats = new List<Seat> { new Seat { Row = "A", Number = 1 } } }
                }
            }
        };

    #endregion
}
