internal class JwtSecurityTokenHandler
{
    private string clientName;
    private string secret;

    public JwtSecurityTokenHandler(string clientName, string secret)
    {
        this.clientName = clientName;
        this.secret = secret;
    }


    //Funtion that generates JWT token based on the client name and the secret key and returns it as string
    public string GenerateToken()
    {
        //Create security key
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        //Create credentials
        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //Create token
        JwtSecurityToken token = new JwtSecurityToken(
            issuer: clientName,
            audience: clientName,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials
        );

        //Return token as string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}