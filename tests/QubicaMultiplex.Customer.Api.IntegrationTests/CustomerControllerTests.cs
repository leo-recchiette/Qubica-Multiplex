using System.Net.Http.Json;
using Dapper;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using QubicaMultiplex.Customer.Api.Models;

namespace QubicaMultiplex.Customer.Api.IntegrationTests;

[TestClass]
[UsesVerify]
public partial class CustomerControllerTests
{
    private readonly WebApplicationFactory<Program> _webApplicationFactory = new();
    private readonly HttpClient _httpClient;

    public CustomerControllerTests()
        => _httpClient = _webApplicationFactory.CreateClient();

    [TestInitialize]
    public async Task ResetDatabaseAsync()
    {
        var connectionString = _webApplicationFactory.Services.GetRequiredService<IConfiguration>().GetConnectionString("Customer");

        await using var connection = new NpgsqlConnection(connectionString);

        await connection.ExecuteAsync("TRUNCATE customer.customer RESTART IDENTITY");
    }

    [TestMethod]
    public async Task Should_RegisterCustomer_When_EmailIsNew()
    {
        var response = await RegisterAsync("user@example.com");

        var actual = await response.Content.ReadAsStringAsync();

        await VerifyJson(actual);
    }

    [TestMethod]
    public async Task Should_ReturnConflict_When_EmailIsAlreadyRegistered()
    {
        await RegisterAsync("user@example.com");

        var response = await RegisterAsync("user@example.com");

        var actual = await response.Content.ReadAsStringAsync();

        await VerifyJson(actual);
    }

    [TestMethod]
    public async Task Should_ReturnBadRequest_When_EmailIsNotValid()
    {
        var response = await RegisterAsync("not-an-email");

        var actual = await response.Content.ReadAsStringAsync();

        await VerifyJson(actual);
    }

    [TestMethod]
    public async Task Should_ReturnCustomer_When_CustomerExists()
    {
        await RegisterAsync("user@example.com");

        var actual = await _httpClient.GetStringAsync("api/Customer/1");

        await VerifyJson(actual);
    }

    [TestMethod]
    public async Task Should_ReturnNotFound_When_CustomerDoesNotExist()
    {
        var response = await _httpClient.GetAsync("api/Customer/99");

        var actual = await response.Content.ReadAsStringAsync();

        await VerifyJson(actual);
    }

    #region Utils

    private Task<HttpResponseMessage> RegisterAsync(string email)
        => _httpClient.PostAsJsonAsync("api/Customer/register", new CustomerRequest { Email = email });

    #endregion
}
