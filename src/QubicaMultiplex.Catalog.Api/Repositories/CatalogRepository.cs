using QubicaMultiplex.Catalog.Api.Models;

namespace QubicaMultiplex.Catalog.Api.Repositories;

public sealed class CatalogRepository
{
    
    public async Task<IEnumerable<Film>> GetFilmsAsync(CancellationToken cancellationToken)
    {
        # Placeholder
        
        return new List<Film>
        {
            new Film
            {
                Id = 1,
                Title = "Kill Bill: Volume 1",
                Description = "Descrizione di Kill Bill: Volume 1",
                Director = "Quentin Tarantino",
                DurationMinutes = 111,
                ReleaseDate = new DateTime(2003, 10, 10)
            },
            new Film
            {
                Id = 2,
                Title = "Il Signore degli Anelli - Il ritorno del re",
                Description = "Descrizione di Il Signore degli Anelli - Il ritorno del re",
                Director = "Peter Jackson",
                DurationMinutes = 201,
                ReleaseDate = new DateTime(2003, 12, 17)
            }
        };
    }
}
