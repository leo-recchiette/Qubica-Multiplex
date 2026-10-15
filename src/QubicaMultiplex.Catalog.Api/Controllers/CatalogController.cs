using Microsoft.AspNetCore.Mvc;
using QubicaMultiplex.Catalog.Api.Models;
using QubicaMultiplex.Catalog.Api.Services;

namespace QubicaMultiplex.Catalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CatalogController : ControllerBase
{
    private readonly ICatalogService _catalogService;

    public CatalogController(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    [HttpGet("allMovies")]
    public async Task<ActionResult<IEnumerable<MovieSummary>>> GetAllMoviesAsync(CancellationToken cancellationToken)
    {
        var movies = await _catalogService.GetAllMoviesAsync(cancellationToken);

        return Ok(movies);
    }

    [HttpGet("details/{id}")]
    public async Task<ActionResult<Movie>> GetMovieDetailsAsync(int id, CancellationToken cancellationToken)
    {
        var movie = await _catalogService.GetMovieDetailsAsync(id, cancellationToken);

        return movie is not null
            ? Ok(movie)
            : NotFound();
    }

    [HttpGet("shows/{movieId}")]
    public async Task<ActionResult<IEnumerable<Showtime>>> GetMovieShowsAsync(int movieId, CancellationToken cancellationToken)
    {
        var shows = await _catalogService.GetMovieShowsAsync(movieId, cancellationToken);

        return shows.Any()
            ? Ok(shows)
            : NotFound();
    }

    [HttpGet("showtime/{showId}")]
    public async Task<ActionResult<ShowtimeDetails>> GetShowtimeDetailsAsync(int showId, CancellationToken cancellationToken)
    {
        var showtime = await _catalogService.GetShowtimeDetailsAsync(showId, cancellationToken);

        return showtime is not null
            ? Ok(showtime)
            : NotFound();
    }
}
