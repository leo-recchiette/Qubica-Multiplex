using System.Net;
using AwesomeAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace QubicaMultiplex.Catalog.Api.IntegrationTests;

[TestClass]
[UsesVerify]
public partial class CatalogControllerTests
{
    private readonly HttpClient _httpClient = new WebApplicationFactory<Program>().CreateClient();

    [TestMethod]
    public async Task Should_ReturnAllMovies_When_AllMoviesAreRequested()
    {
        var actual = await _httpClient.GetStringAsync("api/Catalog/allMovies");

        await VerifyJson(actual);
    }

    [TestMethod]
    public async Task Should_ReturnMovieDetails_When_MovieExists()
    {
        var actual = await _httpClient.GetStringAsync("api/Catalog/details/1");

        await VerifyJson(actual);
    }

    [TestMethod]
    public async Task Should_ReturnNotFound_When_MovieDoesNotExist()
    {
        var actual = await _httpClient.GetAsync("api/Catalog/details/99");

        actual.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task Should_ReturnMovieShows_When_MovieHasShows()
    {
        var actual = await _httpClient.GetStringAsync("api/Catalog/shows/1");

        await VerifyJson(actual);
    }

    [TestMethod]
    public async Task Should_ReturnNotFound_When_MovieHasNoShows()
    {
        var actual = await _httpClient.GetAsync("api/Catalog/shows/99");

        actual.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task Should_ReturnShowtimeWithHall_When_ShowtimeExists()
    {
        var actual = await _httpClient.GetStringAsync("api/Catalog/showtime/1");

        await VerifyJson(actual);
    }

    [TestMethod]
    public async Task Should_ReturnNotFound_When_ShowtimeDoesNotExist()
    {
        var actual = await _httpClient.GetAsync("api/Catalog/showtime/99");

        actual.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
