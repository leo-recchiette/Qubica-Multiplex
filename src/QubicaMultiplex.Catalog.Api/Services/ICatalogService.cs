using QubicaMultiplex.Catalog.Api.Models;

namespace QubicaMultiplex.Catalog.Api.Services;

public interface ICatalogService
{
    Task<IEnumerable<MovieSummary>> GetAllMoviesAsync(CancellationToken cancellationToken);

    Task<Movie?> GetMovieDetailsAsync(int id, CancellationToken cancellationToken);
}
