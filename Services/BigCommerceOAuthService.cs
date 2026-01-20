using Avrhil.BigCommerce.API.Models;
using Newtonsoft.Json;
using System.Text;

namespace Avrhil.BigCommerce.API.Services;

public class BigCommerceOAuthService
{
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _redirectUri;
    private const string AuthUrl = "https://login.bigcommerce.com/oauth2/authorize";
    private const string TokenUrl = "https://login.bigcommerce.com/oauth2/token";

    public BigCommerceOAuthService(string clientId, string clientSecret, string redirectUri)
    {
        _clientId = clientId;
        _clientSecret = clientSecret;
        _redirectUri = redirectUri;
    }

    public string GetAuthorizationUrl(string scope = "store_v2_products store_v2_orders store_inventory store_locations")
    {
        return $"{AuthUrl}?client_id={_clientId}&redirect_uri={Uri.EscapeDataString(_redirectUri)}&response_type=code&scope={Uri.EscapeDataString(scope)}";
    }

    public async Task<OAuthTokenResponse> ExchangeCodeForTokenAsync(string code, string context, string scope)
    {
        using var httpClient = new HttpClient();
        
        var tokenRequest = new OAuthTokenRequest
        {
            client_id = _clientId,
            client_secret = _clientSecret,
            code = code,
            scope = scope,
            redirect_uri = _redirectUri,
            context = context
        };

        var json = JsonConvert.SerializeObject(tokenRequest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await httpClient.PostAsync(TokenUrl, content);
        response.EnsureSuccessStatusCode();
        
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<OAuthTokenResponse>(responseContent);
    }
}
