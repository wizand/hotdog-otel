

public class DtoLoginModel
{
    //Constructor that takes in the client name and secret key
    public DtoLoginModel(string clientName, string clientSecretKey)
    {
        ClientName = clientName;
        ClientSecretKey = clientSecretKey;
    }

    public string ClientName { get; set; }
    public string ClientSecretKey { get; set; }
}