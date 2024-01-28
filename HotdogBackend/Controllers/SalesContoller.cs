using Microsoft.AspNetCore.Mvc;

namespace HotdogBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class SalesController : ControllerBase
{
    private readonly ILogger<SalesController> _logger;

    public SalesController(ILogger<SalesController> logger)
    {
        _logger = logger;
    }

    [HttpPut(Name = "PutOrder")]
    public IActionResult Put()
    {
        _logger.LogInformation("Order placed");
        return Ok("Order placed");
    }


}
