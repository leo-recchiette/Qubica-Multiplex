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
        var response = ApiResponse<ShowtimeDetails>(HttpStatusCode.NotFound);

        var actual = () => response.GetShowtimeOrThrow(999);

        actual.Should().Throw<ShowNotFoundException>().WithMessage("Show 999 does not exist.");
    }

    [TestMethod]
    public async Task Should_ThrowApiException_When_ShowtimeResponseIsAnError()
    {
        var response = await ErrorApiResponse<ShowtimeDetails>(HttpStatusCode.InternalServerError);

        var actual = () => response.GetShowtimeOrThrow(13);

        actual.Should().Throw<ApiException>().Which.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    [TestMethod]
    public void Should_ReturnCustomer_When_ResponseIsSuccessful()
    {
        var response = ApiResponse(HttpStatusCode.OK, Customer());

        var actual = response.GetCustomerOrThrow(1);

        var expected = Customer();

        actual.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void Should_ThrowCustomerNotFoundException_When_ResponseIsNotFound()
    {
        var response = ApiResponse<Customer>(HttpStatusCode.NotFound);

        var actual = () => response.GetCustomerOrThrow(999);

        actual.Should().Throw<CustomerNotFoundException>().WithMessage("Customer 999 does not exist.");
    }

    [TestMethod]
    public async Task Should_ThrowApiException_When_CustomerResponseIsAnError()
    {
        var response = await ErrorApiResponse<Customer>(HttpStatusCode.InternalServerError);

        var actual = () => response.GetCustomerOrThrow(1);

        actual.Should().Throw<ApiException>().Which.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    #region Utils

    private static IApiResponse<T> ApiResponse<T>(HttpStatusCode statusCode, T? content = null) where T : class
        => new ApiResponse<T>(
            HttpResponse(statusCode),
            content,
            new RefitSettings());

    private static async Task<IApiResponse<T>> ErrorApiResponse<T>(HttpStatusCode statusCode) where T : class
    {
        var httpResponse = HttpResponse(statusCode);
        var error = await ApiException.Create(httpResponse.RequestMessage!, HttpMethod.Get, httpResponse, new RefitSettings());

        return new ApiResponse<T>(httpResponse, null, new RefitSettings(), error);
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

    private static Customer Customer()
        => new Customer
        {
            Id = 1,
            Email = "leonardo@example.com"
        };

    #endregion
}
