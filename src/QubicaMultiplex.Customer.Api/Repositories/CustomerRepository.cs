using Dapper;
using Npgsql;
using QubicaMultiplex.Customer.Api.Exceptions;
using QubicaMultiplex.Customer.Api.Models;
using QubicaMultiplex.Customer.Api.Repositories.Interfaces;

namespace QubicaMultiplex.Customer.Api.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly string _connectionString;

    public CustomerRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Customer")!;
    }

    public async Task<CustomerDetails> GetCustomerAsync(int customerId, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT id,
                   email,
                   created_at AS CreatedAt
            FROM customer.customer
            WHERE id = @CustomerId
            """;

        await using var connection = new NpgsqlConnection(_connectionString);

        return await connection.QueryFirstOrDefaultAsync<CustomerDetails>(new CommandDefinition(sql, new { CustomerId = customerId }, cancellationToken: cancellationToken))
            ?? throw new CustomerNotFoundException(customerId);
    }

    public async Task<CustomerDetails> CreateCustomerAsync(string email, CancellationToken cancellationToken)
    {
        const string sql = """
            INSERT INTO customer.customer (email)
            VALUES (@Email)
            RETURNING id,
                      email,
                      created_at AS CreatedAt
            """;

        await using var connection = new NpgsqlConnection(_connectionString);

        try
        {
            return await connection.QuerySingleAsync<CustomerDetails>(new CommandDefinition(sql, new { Email = email }, cancellationToken: cancellationToken));
        }
        catch (PostgresException exception) when (exception.SqlState == PostgresErrorCodes.UniqueViolation)
        {
            throw new EmailAlreadyRegisteredException(email, exception);
        }
    }
}
