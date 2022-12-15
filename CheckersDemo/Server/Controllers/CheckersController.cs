using Microsoft.AspNetCore.Mvc;

namespace CheckersDemo.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class CheckersController : ControllerBase
{
    public CheckersController()
    {

    }

    [HttpGet("GetTables")]
    public IEnumerable<string> GetTables()
    {

    }
}