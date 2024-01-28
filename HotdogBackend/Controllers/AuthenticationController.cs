using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace HotdogBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    

    private readonly ILogger<AuthenticationController> _logger;
    private readonly IConfiguration _configuration;

    public AuthenticationController(ILogger<AuthenticationController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    [HttpPost(Name = "Authenticate")]
    public IActionResult Atuhenticate(DtoLoginModel clientLogin)
    {
        _logger.LogInformation("Authenticating client " + clientLogin.ClientName);

        string? secret = _configuration["AuthorizedClients:" + clientLogin.ClientName]; 

        if (secret == null)
        {
            _logger.LogInformation("Client " + clientLogin.ClientName + " not found");
            return Unauthorized("Client not found");
        }

        if (secret != clientLogin.ClientSecretKey)
        {
            _logger.LogInformation("Client " + clientLogin.ClientName + " provided wrong secret key");
            return Unauthorized("Wrong secret key");
        }

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler(clientLogin.ClientName, secret);

        

        return Ok(tokenHandler.GenerateToken());
    }
}

