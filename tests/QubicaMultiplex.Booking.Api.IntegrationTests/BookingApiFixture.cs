using System.Net;
using AwesomeAssertions.Extensions;
using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using NSubstitute;
using QubicaMultiplex.Booking.Api.Clients;
using QubicaMultiplex.Booking.Api.Models;
using Refit;

namespace QubicaMultiplex.Booking.Api.IntegrationTests;

public class BookingApiFixture : WebApplicationFactory<Program>
{
    private readonly ICatalogClient _catalogClient = Substitute.For<ICatalogClient>();
    private readonly ICustomerClient _customerClient = Substitute.For<ICustomerClient>();

    public BookingApiFixture()
    {
        _catalogClient
            .GetShowtimeDetailsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(_ => ApiResponse<ShowtimeDetails>(HttpStatusCode.NotFound));

        _catalogClient
            .GetShowtimeDetailsAsync(Arg.Is<int>(showId => showId == 1 || showId == 2), Arg.Any<CancellationToken>())
            .Returns(call => ApiResponse(HttpStatusCode.OK, ShowtimeDetails(call.Arg<int>())));

        _customerClient
            .GetCustomerAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(_ => ApiResponse<Customer>(HttpStatusCode.NotFound));

        _customerClient
            .GetCustomerAsync(1, Arg.Any<CancellationToken>())
            .Returns(_ => ApiResponse(HttpStatusCode.OK, Customer()));
    }

    public async Task ResetDatabaseAsync()
    {
        var connectionString = Services.GetRequiredService<IConfiguration>().GetConnectionString("Booking");

        await using var connection = new NpgsqlConnection(connectionString);

        await connection.ExecuteAsync("TRUNCATE booking.booking, booking.booking_seat RESTART IDENTITY");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
        => builder.ConfigureTestServices(services =>
        {
            services.AddSingleton(_catalogClient);
            services.AddSingleton(_customerClient);
        });

    #region Utils

    private static IApiResponse<T> ApiResponse<T>(HttpStatusCode statusCode, T? content = null) where T : class
        => new ApiResponse<T>(
            new HttpResponseMessage(statusCode) { RequestMessage = new HttpRequestMessage() },
            content,
            new RefitSettings());

    private static ShowtimeDetails ShowtimeDetails(int showId)
        => new ShowtimeDetails
        {
            ShowId = showId,
            StartsAt = 20.October(2026).At(21, 30),
            Hall = new Hall { Name = "Sala 1", LastRow = "B", SeatsPerRow = 2 }
        };

    private static Customer Customer()
        => new Customer
        {
            Id = 1,
            Email = "user@example.com"
        };

    #endregion
}
