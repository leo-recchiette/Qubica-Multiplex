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
    public async Task<ActionResult<IEnumerable<Film>>> GetAllFilmsAsync(CancellationToken cancellationToken)
    {
        var films = await _catalogService.GetAllFilmsAsync(cancellationToken);
        
        return films.Any()
            ? Ok(films)
            : NotFound();
    }
}
