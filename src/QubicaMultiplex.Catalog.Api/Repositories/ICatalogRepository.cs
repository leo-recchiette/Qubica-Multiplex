using QubicaMultiplex.Catalog.Api.Models;

namespace QubicaMultiplex.Catalog.Api.Repositories;

public interface ICatalogRepository
{
    Task<IEnumerable<MovieSummary>> GetAllMoviesAsync(CancellationToken cancellationToken);

    Task<Movie?> GetMovieDetailsAsync(int id, CancellationToken cancellationToken);

    Task<IEnumerable<Showtime>> GetMovieShowsAsync(int movieId, CancellationToken cancellationToken);
}
