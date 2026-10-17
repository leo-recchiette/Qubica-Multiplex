using System.Net;
using AwesomeAssertions;
using AwesomeAssertions.Extensions;
using QubicaMultiplex.Booking.Api.Exceptions;
using QubicaMultiplex.Booking.Api.Extensions;
using QubicaMultiplex.Booking.Api.Models;
using Refit;

namespace QubicaMultiplex.Booking.Api.Tests;

[TestClass]
public class ApiResponseExtensionsTests
{
    [TestMethod]
    public void Should_ReturnShowtime_When_ResponseIsSuccessful()
    {
        var response = ApiResponse(HttpStatusCode.OK, ShowtimeDetails());

        var actual = response.GetShowtimeOrThrow(13);

        var expected = ShowtimeDetails();

        actual.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void Should_ThrowShowNotFoundException_When_ResponseIsNotFound()
    {
        var response = ApiResponse(HttpStatusCode.NotFound);

        var actual = () => response.GetShowtimeOrThrow(999);

        actual.Should().Throw<ShowNotFoundException>().WithMessage("Show 999 does not exist.");
    }

    [TestMethod]
    public async Task Should_ThrowApiException_When_ResponseIsAnError()
    {
        var response = await ErrorApiResponse(HttpStatusCode.InternalServerError);

        var actual = () => response.GetShowtimeOrThrow(13);

        actual.Should().Throw<ApiException>().Which.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    #region Utils

    private static IApiResponse<ShowtimeDetails> ApiResponse(HttpStatusCode statusCode, ShowtimeDetails? content = null)
        => new ApiResponse<ShowtimeDetails>(
            HttpResponse(statusCode), 
            content, 
            new RefitSettings());

    private static async Task<IApiResponse<ShowtimeDetails>> ErrorApiResponse(HttpStatusCode statusCode)
    {
        var httpResponse = HttpResponse(statusCode);
        var error = await ApiException.Create(httpResponse.RequestMessage!, HttpMethod.Get, httpResponse, new RefitSettings());

        return new ApiResponse<ShowtimeDetails>(httpResponse, null, new RefitSettings(), error);
    }

    private static HttpResponseMessage HttpResponse(HttpStatusCode statusCode)
        => new HttpResponseMessage(statusCode) { RequestMessage = new HttpRequestMessage() };

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

    #endregion
}
