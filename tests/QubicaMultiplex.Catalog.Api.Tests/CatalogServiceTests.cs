using NSubstitute;
using QubicaMultiplex.Catalog.Api.Models;
using QubicaMultiplex.Catalog.Api.Repositories;
using QubicaMultiplex.Catalog.Api.Services;

namespace QubicaMultiplex.Catalog.Api.Tests;

[TestClass]
public class CatalogServiceTests
{
    private readonly ICatalogRepository _catalogRepository = Substitute.For<ICatalogRepository>();
    private readonly CatalogService _sut;

    public CatalogServiceTests()
        => _sut = new CatalogService(_catalogRepository);

    [TestMethod]
    public async Task Should_ReturnShowtimeWithHall_When_ShowtimeExists()
    {
        _catalogRepository
            .GetShowtimeDetailsAsync(13, Arg.Any<CancellationToken>())
            .Returns(ShowtimeDetailsEntity());

        var actual = await _sut.GetShowtimeDetailsAsync(13, CancellationToken.None);

        var expected = ShowtimeDetails();

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public async Task Should_ReturnNull_When_ShowtimeDoesNotExist()
    {
        _catalogRepository
            .GetShowtimeDetailsAsync(999, Arg.Any<CancellationToken>())
            .Returns((ShowtimeDetailsEntity?)null);

        var actual = await _sut.GetShowtimeDetailsAsync(999, CancellationToken.None);

        Assert.IsNull(actual);
    }

    #region Utils

    private static ShowtimeDetailsEntity ShowtimeDetailsEntity()
        => new ShowtimeDetailsEntity
        {
            ShowId = 13,
            StartsAt = new DateTime(2026, 10, 20, 21, 30, 0),
            HallName = "Hall 1",
            LastRow = "H",
            SeatsPerRow = 30
        };

    private static ShowtimeDetails ShowtimeDetails()
        => new ShowtimeDetails
        {
            ShowId = 13,
            StartsAt = new DateTime(2026, 10, 20, 21, 30, 0),
            Hall = Hall()
        };

    private static Hall Hall()
        => new Hall
        {
            Name = "Hall 1",
            LastRow = "H",
            SeatsPerRow = 30
        };

    #endregion
}
