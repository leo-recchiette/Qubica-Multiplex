using QubicaMultiplex.Catalog.Api.Models;
using QubicaMultiplex.Catalog.Api.Repositories;

namespace QubicaMultiplex.Catalog.Api.Services;

public class CatalogService : ICatalogService
{
    private readonly ICatalogRepository _catalogRepository;

    public CatalogService(ICatalogRepository catalogRepository)
        => _catalogRepository = catalogRepository;
    
    public async Task<IEnumerable<MovieSummary>> GetAllMoviesAsync(CancellationToken cancellationToken)
        => await _catalogRepository.GetAllMoviesAsync(cancellationToken);

    public async Task<Movie?> GetMovieDetailsAsync(int id, CancellationToken cancellationToken)
        => await _catalogRepository.GetMovieDetailsAsync(id, cancellationToken);

    public async Task<IEnumerable<Showtime>> GetMovieShowsAsync(int movieId, CancellationToken cancellationToken)
        => await _catalogRepository.GetMovieShowsAsync(movieId, cancellationToken);

    public async Task<ShowtimeDetails?> GetShowtimeDetailsAsync(int showId, CancellationToken cancellationToken)
    {
        var entity = await _catalogRepository.GetShowtimeDetailsAsync(showId, cancellationToken);

        return entity is null
            ? null
            : new ShowtimeDetails
            {
                ShowId = entity.ShowId,
                StartsAt = entity.StartsAt,
                Hall = new Hall
                {
                    Name = entity.HallName,
                    LastRow = entity.LastRow,
                    SeatsPerRow = entity.SeatsPerRow
                }
            };
    }
}
