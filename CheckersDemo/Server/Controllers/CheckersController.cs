using CheckersDemo.Server.Data;
using Microsoft.AspNetCore.Mvc;

namespace CheckersDemo.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class CheckersController : ControllerBase
{
    private readonly TableManager _tableManager;

    public CheckersController(TableManager tableManager)
    {
        _tableManager = tableManager;
    }

    [HttpGet("GetTables")]
    public IEnumerable<string> GetTables()
    {
        return  _tableManager.GetTables();
    }
}