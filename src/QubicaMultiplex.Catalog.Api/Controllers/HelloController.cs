using Microsoft.AspNetCore.Mvc;

namespace QubicaMultiplex.Catalog.Api.Controllers;

[ApiController]
[Route("api/hello")]
public sealed class HelloController : ControllerBase
{
    [HttpGet]
    public string Get() => "Hello World!";
}
