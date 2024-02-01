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

        string? apiKey = _configuration["ApiKey"];

        if ( apiKey == null)
        {
            _logger.LogInformation("Api key not found");
            return StatusCode(StatusCodes.Status500InternalServerError, "Api key not found");
        }   

        try
        {

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler(clientLogin.ClientName, secret, apiKey);
            return Ok(tokenHandler.GenerateToken());
        } catch (Exception e)
        {
            _logger.LogError("Error generating token: " + e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "Error generating token");
        }
    }
}

