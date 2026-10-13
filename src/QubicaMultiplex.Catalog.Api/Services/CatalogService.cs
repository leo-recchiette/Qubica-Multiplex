using QubicaMultiplex.Catalog.Api.Models;
using QubicaMultiplex.Catalog.Api.Repositories;

namespace QubicaMultiplex.Catalog.Api.Services;

public sealed class CatalogService
{
    private readonly CatalogRepository _catalogRepository;

    public CatalogService(CatalogRepository catalogRepository)
    {
        _catalogRepository = catalogRepository;
    }

    public async Task<IEnumerable<Film>> GetLineUpAsync(CancellationToken cancellationToken)
    {
        return await _catalogRepository.GetFilmsAsync(cancellationToken);
    }
}
