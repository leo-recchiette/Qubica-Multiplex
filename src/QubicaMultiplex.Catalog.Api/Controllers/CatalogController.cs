using Microsoft.AspNetCore.Mvc;
using QubicaMultiplex.Catalog.Api.Models;
using QubicaMultiplex.Catalog.Api.Services;

namespace QubicaMultiplex.Catalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CatalogController : ControllerBase
{
    private readonly CatalogService _catalogService;

    public CatalogController(CatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    [HttpGet("allFilms")]
    public async Task<ActionResult<IEnumerable<string>>> GetAllFilmsAsync(CancellationToken cancellationToken)
    {
        var films = await _catalogService.GetAllFilmsAsync(cancellationToken);
        
        return Ok(films.Select(f => f.Title));
    }

    [HttpGet("details/{title}")]
    public async Task<ActionResult<Film>> GetFilmDetailsAsync(string title, CancellationToken cancellationToken)
    {
        var film = await _catalogService.GetFilmDetailsAsync(title, cancellationToken);

        return film is not null 
            ? Ok(film) 
            : NotFound();
    }
}
