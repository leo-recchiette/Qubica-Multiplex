using QubicaMultiplex.Catalog.Api.Models;
using QubicaMultiplex.Catalog.Api.Repositories;

namespace QubicaMultiplex.Catalog.Api.Services;

public class CatalogService : ICatalogService
{
    private readonly ICatalogRepository _catalogRepository;

    public CatalogService(ICatalogRepository catalogRepository)
    {
        _catalogRepository = catalogRepository;
    }

    public async Task<IEnumerable<Movie>> GetAllMoviesAsync(CancellationToken cancellationToken)
    {
        return await _catalogRepository.GetAllMoviesAsync(cancellationToken);
    }

    public async Task<Movie?> GetMovieDetailsAsync(string title, CancellationToken cancellationToken)
    {
        return await _catalogRepository.GetMovieDetailsAsync(title, cancellationToken);
    }
}
