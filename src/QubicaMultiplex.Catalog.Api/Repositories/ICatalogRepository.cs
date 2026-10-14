using QubicaMultiplex.Catalog.Api.Models;

namespace QubicaMultiplex.Catalog.Api.Repositories;

public interface ICatalogRepository
{
    Task<IEnumerable<Movie>> GetAllMoviesAsync(CancellationToken cancellationToken);

    Task<Movie?> GetMovieDetailsAsync(string title, CancellationToken cancellationToken);
}
