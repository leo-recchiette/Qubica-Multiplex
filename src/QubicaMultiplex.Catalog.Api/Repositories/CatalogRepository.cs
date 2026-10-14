using QubicaMultiplex.Catalog.Api.Models;

namespace QubicaMultiplex.Catalog.Api.Repositories;

public sealed class CatalogRepository
{
    
    public async Task<IEnumerable<Movie>> GetAllMoviesAsync(CancellationToken cancellationToken)
    {
        // Placeholder
        
        return new List<Movie>
        {
            new Movie
            {
                Id = 1,
                Title = "Kill Bill: Volume 1",
                Description = "Descrizione di Kill Bill: Volume 1",
                Director = "Quentin Tarantino",
                DurationMinutes = 111,
                ReleaseDate = new DateTime(2003, 10, 10)
            },
            new Movie
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

    public async Task<Movie?> GetMovieDetailsAsync(string title, CancellationToken cancellationToken)
    {
        var movies = await GetAllMoviesAsync(cancellationToken);

        return movies.FirstOrDefault(m => string.Equals(m.Title, title, StringComparison.OrdinalIgnoreCase));
    }
}
