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

    public async Task<IEnumerable<BookingDetailsEntity>> GetCustomerBookingsAsync(int customerId, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT b.id AS BookingId,
                   b.created_at AS CreatedAt,
                   s.showtime_id AS ShowId,
                   s.row_label AS Row,
                   s.seat_number AS Number
            FROM booking.booking b
            JOIN booking.booking_seat s ON s.booking_id = b.id
            WHERE b.customer_id = @CustomerId
            ORDER BY b.id, s.showtime_id, s.row_label, s.seat_number
            """;

        await using var connection = new NpgsqlConnection(_connectionString);

        return await connection.QueryAsync<BookingDetailsEntity>(new CommandDefinition(sql, new { CustomerId = customerId }, cancellationToken: cancellationToken));
    }

    public async Task<BookingEntity> CreateBookingAsync(BookingRequest request, CancellationToken cancellationToken)
    {
        const string bookingSql = """
            INSERT INTO booking.booking (customer_id, customer_email)
            VALUES (@CustomerId, @CustomerEmail)
            RETURNING id,
                      created_at AS CreatedAt
            """;

        const string bookingSeatSql = """
            INSERT INTO booking.booking_seat (booking_id, showtime_id, row_label, seat_number)
            VALUES (@BookingId, @ShowId, @Row, @Number)
            """;

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        var booking = await connection.QuerySingleAsync<BookingEntity>(new CommandDefinition(
            bookingSql,
            new { CustomerId = request.Customer.Id, CustomerEmail = request.Customer.Email },
            transaction,
            cancellationToken: cancellationToken));

        foreach (var show in request.Shows)
        {
            foreach (var seat in show.Seats.ChosenSeats)
            {
                await connection.ExecuteAsync(new CommandDefinition(
                    bookingSeatSql,
                    new { BookingId = booking.Id, show.ShowId, seat.Row, seat.Number },
                    transaction,
                    cancellationToken: cancellationToken));
            }
        }

        await transaction.CommitAsync(cancellationToken);

        return booking;
    }

    public async Task<bool> DeleteBookingAsync(int bookingId, CancellationToken cancellationToken)
    {
        const string sql = "DELETE FROM booking.booking WHERE id = @BookingId";

        await using var connection = new NpgsqlConnection(_connectionString);

        var deletedRows = await connection.ExecuteAsync(new CommandDefinition(sql, new { BookingId = bookingId }, cancellationToken: cancellationToken));

        return deletedRows > 0;
    }
}
