using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text;

namespace HotdogFrontend.ManagersAndHandlers
{
    public class JwtTokenHandler
    {

        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        public JwtTokenHandler(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;

        }

        private JwtSecurityToken? cachedToken = null;

        //Create a method that will fetch JWT token from the HotdogBackend service's AuthenticationController's endpoint /api/Authentication/Authenticate
        //The method uses IConfiguration to fetch the secret key from appsettings.json
        //The method uses HttpClient to make the request to the AuthenticationController
        public async Task<string> GetJwtToken()
        {



            if ( isCachedTokenIsNullOrExpired() )
            {
                string? hotdogBackendUrl = _configuration["HotdogService:HotdogBackendUrl"];
                if ( hotdogBackendUrl == null )
                {
                    throw new InvalidDataException("Lacking HotdogBackendUrl");
                }   

                string authenticationUri = hotdogBackendUrl + "/Authentication";

                //Generate Http reequest for sending a POST method to the AuthenticationController. Use HttpContent to add the client name and secret key to the request body


                string? jwtClientName = _configuration["HotdogService:ClientName"];
                string? jwtClientSecret = _configuration["HotdogService:ClientSecret"];
                if ( jwtClientName == null || jwtClientSecret == null) 
                {
                    throw new InvalidDataException("Lacking Jwt client name or secret key");
                }

                DtoLoginModel loginModel = new DtoLoginModel(jwtClientName, jwtClientSecret);

                HttpContent content = new StringContent(System.Text.Json.JsonSerializer.Serialize(loginModel), Encoding.UTF8, "application/json");


                //Create a HttpClient that will send the request
                HttpClient client = _httpClientFactory.CreateClient();
                var response = await client.PostAsync( new Uri(authenticationUri), content);

                if ( response.IsSuccessStatusCode )
                {
                    //Update the cached token with the response body content
                    cachedToken = new JwtSecurityToken(await response.Content.ReadAsStringAsync());
                } else
                {
                    throw new HttpRequestException("Failed to authenticate with the backend service");
                }

            }

            return cachedToken!.ToString();
        }


        //Create a method that will check if the cached token is null or expired
        private bool isCachedTokenIsNullOrExpired()
        {
            if (cachedToken == null)
            {
                return true;
            }

            if ( cachedToken.ValidTo < DateTime.Now )
            {
                return true;
            }

            return false;
        }

    }
}
