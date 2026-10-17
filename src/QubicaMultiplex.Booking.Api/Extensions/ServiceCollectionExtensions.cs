using QubicaMultiplex.Booking.Api.Clients;
using QubicaMultiplex.Booking.Api.Repositories;
using QubicaMultiplex.Booking.Api.Services;
using Refit;

namespace QubicaMultiplex.Booking.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBookingServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<ISeatService, SeatService>();
        services.AddScoped<IBookingRepository, BookingRepository>();

        services.AddRefitGeneratedClient<ICatalogClient>()
            .ConfigureHttpClient(client => client.BaseAddress = new Uri(configuration["CatalogApi:BaseUrl"]!));

        return services;
    }
}
