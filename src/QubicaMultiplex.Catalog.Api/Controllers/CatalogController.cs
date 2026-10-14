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
    public async Task<ActionResult<IEnumerable<string>>> GetAllMoviesAsync(CancellationToken cancellationToken)
    {
        var movies = await _catalogService.GetAllMoviesAsync(cancellationToken);
        
        return Ok(movies.Select(m => m.Title));
    }

    [HttpGet("details/{title}")]
    public async Task<ActionResult<Movie>> GetMovieDetailsAsync(string title, CancellationToken cancellationToken)
    {
        var movie = await _catalogService.GetMovieDetailsAsync(title, cancellationToken);

        return movie is not null 
            ? Ok(movie) 
            : NotFound();
    }
}
