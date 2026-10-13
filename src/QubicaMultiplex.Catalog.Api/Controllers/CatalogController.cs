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
        //Placeholder
        return new Film
            {
                Id = 1,
                Title = "Kill Bill: Volume 1",
                Description = "Descrizione di Kill Bill: Volume 1",
                Director = "Quentin Tarantino",
                DurationMinutes = 111,
                ReleaseDate = new DateTime(2003, 10, 10)
            };
    }
}
