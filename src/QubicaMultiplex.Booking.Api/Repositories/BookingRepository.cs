using Dapper;
using Npgsql;
using QubicaMultiplex.Booking.Api.Models;

namespace QubicaMultiplex.Booking.Api.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly string _connectionString;

    public BookingRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Booking")!;
    }

    public async Task<IEnumerable<Seat>> GetBookedSeatsAsync(int showId, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT row_label AS Row,
                   seat_number AS Number
            FROM booking.booking_seat
            WHERE showtime_id = @ShowId
            """;

        await using var connection = new NpgsqlConnection(_connectionString);

        return await connection.QueryAsync<Seat>(new CommandDefinition(sql, new { ShowId = showId }, cancellationToken: cancellationToken));
    }

    public async Task<bool> DeleteBookingAsync(int id, CancellationToken cancellationToken)
    {
        const string sql = "DELETE FROM booking.booking WHERE id = @Id";

        await using var connection = new NpgsqlConnection(_connectionString);

        var deletedRows = await connection.ExecuteAsync(new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));

        return deletedRows > 0;
    }
}
