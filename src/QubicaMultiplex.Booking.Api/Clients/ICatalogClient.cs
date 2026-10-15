using QubicaMultiplex.Booking.Api.Models;
using Refit;

namespace QubicaMultiplex.Booking.Api.Clients;

public interface ICatalogClient
{
    [Get("/api/Catalog/showtime/{showId}")]
    Task<IApiResponse<ShowtimeDetails>> GetShowtimeDetailsAsync(int showId, CancellationToken cancellationToken);
}
