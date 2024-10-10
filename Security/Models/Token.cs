namespace Security.Models;

public class Token
{
    public Token(bool authenticated, string created, string expiration, string accessToken)
    {
        Authenticated = authenticated;
        Created = created;
        Expiration = expiration;
        AccessToken = accessToken;
    }

    public bool Authenticated { get; set; }
    public string Created { get; set; }
    public string Expiration { get; set; }
    public string AccessToken { get; set; }
}
