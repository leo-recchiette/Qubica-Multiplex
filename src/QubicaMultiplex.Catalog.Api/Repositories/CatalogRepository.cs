using Dapper;
using Npgsql;
using QubicaMultiplex.Catalog.Api.Models;

namespace QubicaMultiplex.Catalog.Api.Repositories;

public class CatalogRepository : ICatalogRepository
{
    private readonly string _connectionString;

    public CatalogRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Catalog")!;
    }

    public async Task<IEnumerable<MovieSummary>> GetAllMoviesAsync(CancellationToken cancellationToken)
    {
        const string sql = "SELECT id, title FROM catalog.movie";

        await using var connection = new NpgsqlConnection(_connectionString);

        return await connection.QueryAsync<MovieSummary>(new CommandDefinition(sql, cancellationToken: cancellationToken));
    }

    public async Task<Movie?> GetMovieDetailsAsync(int id, CancellationToken cancellationToken)
    {
        const string sql = """
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
            WHERE m.id = @Id
            """;

        await using var connection = new NpgsqlConnection(_connectionString);

        return await connection.QueryFirstOrDefaultAsync<Movie>(new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
    }

    public async Task<IEnumerable<Showtime>> GetMovieShowsAsync(int movieId, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT s.id AS ShowId,
                   s.starts_at AS StartsAt,
                   h.name AS hall
            FROM catalog.showtime s
            JOIN catalog.hall h ON h.id = s.hall_id
            WHERE s.movie_id = @MovieId
            ORDER BY s.starts_at
            """;

        await using var connection = new NpgsqlConnection(_connectionString);

        return await connection.QueryAsync<Showtime>(new CommandDefinition(sql, new { MovieId = movieId }, cancellationToken: cancellationToken));
    }

    public async Task<ShowtimeDetailsEntity?> GetShowtimeDetailsAsync(int showId, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT s.id AS ShowId,
                   s.starts_at AS StartsAt,
                   h.name AS HallName,
                   h.last_row AS LastRow,
                   h.seats_per_row AS SeatsPerRow
            FROM catalog.showtime s
            JOIN catalog.hall h ON h.id = s.hall_id
            WHERE s.id = @ShowId
            """;

        await using var connection = new NpgsqlConnection(_connectionString);

        return await connection.QueryFirstOrDefaultAsync<ShowtimeDetailsEntity>(new CommandDefinition(sql, new { ShowId = showId }, cancellationToken: cancellationToken));
    }
}
