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
    public IEnumerable<DtoOrderItem> GetPriceTable()
    {
        return new List<DtoOrderItem>
            {
                new DtoOrderItem { Name = "Hotdog normal", Price = 9.50f },
                new DtoOrderItem { Name = "Hotdog extra", Price = 13.50f },
                new DtoOrderItem { Name = "Hotdog vege", Price = 10.50f },
                new DtoOrderItem { Name = "Hotdog kids", Price = 7.50f },
                new DtoOrderItem { Name = "Hotdog gluten free", Price = 13.50f }

            };
    }
}
