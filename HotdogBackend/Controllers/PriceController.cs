using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace HotdogBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class PriceController : ControllerBase
{
    

    private readonly ILogger<PriceController> _logger;

    public PriceController(ILogger<PriceController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "PriceTable")]
    public IEnumerable<KeyValuePair<string, float>> GetPriceTable()
    {
        List<KeyValuePair<string, float>> priceTable = new List<KeyValuePair<string, float>>
        {
            new KeyValuePair<string, float>("Hotdog", 2.00f),
            new KeyValuePair<string, float>("Soda", 1.50f),
            new KeyValuePair<string, float>("Fries", 1.00f)
        };
        
        return priceTable; 
    }
}
