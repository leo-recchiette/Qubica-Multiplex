using QubicaMultiplex.Catalog.Api.Models;

namespace QubicaMultiplex.Catalog.Api.Services;

public interface ICatalogService
{
    Task<IEnumerable<Movie>> GetAllMoviesAsync(CancellationToken cancellationToken);

    Task<Movie?> GetMovieDetailsAsync(string title, CancellationToken cancellationToken);
}
