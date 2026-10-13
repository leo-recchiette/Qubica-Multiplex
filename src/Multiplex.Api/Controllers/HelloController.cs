using Microsoft.AspNetCore.Mvc;

namespace Multiplex.Api.Controllers;

[ApiController]
[Route("api/hello")]
public sealed class HelloController : ControllerBase
{
    [HttpGet]
    public string Get() => "Hello World!";
}
