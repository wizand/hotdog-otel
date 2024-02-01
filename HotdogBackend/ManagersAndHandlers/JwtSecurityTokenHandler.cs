using Microsoft.IdentityModel.Tokens;

using OtelCommon;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

internal class JwtSecurityTokenHandler
{
    private string _clientName;
    private string _secret;
    private string _apiKey;

    public JwtSecurityTokenHandler(string clientName, string secret, string apiKey)
    {
        this._clientName = clientName;
        this._secret = secret;
        this._apiKey = apiKey;
    }


    //Funtion that generates JWT token based on the client name and the secret key and returns it as string
    public string GenerateToken()
    {
        //Create security key
        byte[] keyBytes = Encoding.UTF8.GetBytes(_apiKey);
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(keyBytes);

        //Create credentials
        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


        Claim[] claims =
            [
                new Claim(type: JwtRegisteredClaimNames.Sub, value: _clientName+ "" + Guid.NewGuid()),
                new Claim(type: JwtRegisteredClaimNames.Jti, value: Guid.NewGuid().ToString()),
                new Claim(type: "sid", value: ServiceNames.HotDogService),
            ];

        //Create token
        JwtSecurityToken token = new JwtSecurityToken(
            issuer: ServiceNames.HotDogBackend,
            audience: _clientName,
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials
        );

        string tokenString = token.ToString();
        return tokenString;
    }
}