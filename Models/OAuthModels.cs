namespace Avrhil.BigCommerce.API.Models;

public class OAuthTokenRequest
{
    public string client_id { get; set; }
    public string client_secret { get; set; }
    public string code { get; set; }
    public string scope { get; set; }
    public string grant_type { get; set; } = "authorization_code";
    public string redirect_uri { get; set; }
    public string context { get; set; }
}

public class OAuthTokenResponse
{
    public string access_token { get; set; }
    public string scope { get; set; }
    public dynamic user { get; set; }
    public dynamic owner { get; set; }
    public string context { get; set; }
    public string account_uuid { get; set; }
}
