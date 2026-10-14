using Dapper;
using Npgsql;
using QubicaMultiplex.Catalog.Api.Models;

namespace QubicaMultiplex.Catalog.Api.Repositories;

public class CatalogRepository : ICatalogRepository
{
    private const string SelectMovies = """
        SELECT m.id,
               m.title,
               m.description,
               d.first_name || ' ' || d.last_name AS director,
               g.name AS genre,
               m.duration_minutes AS DurationMinutes,
               m.release_date AS ReleaseDate
        FROM catalog.movie m
        JOIN catalog.director d ON d.id = m.director_id
        JOIN catalog.genre g ON g.id = m.genre_id
        """;

    private readonly string _connectionString;

    public CatalogRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Catalog")!;
    }

    public async Task<IEnumerable<Movie>> GetAllMoviesAsync(CancellationToken cancellationToken)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        return await connection.QueryAsync<Movie>(new CommandDefinition(SelectMovies, cancellationToken: cancellationToken));
    }

    public async Task<Movie?> GetMovieDetailsAsync(string title, CancellationToken cancellationToken)
    {
        const string sql = SelectMovies + " WHERE m.title = @Title";

        await using var connection = new NpgsqlConnection(_connectionString);

        return await connection.QueryFirstOrDefaultAsync<Movie>(new CommandDefinition(sql, new { Title = title }, cancellationToken: cancellationToken));
    }
}
