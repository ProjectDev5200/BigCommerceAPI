using Avrhil.BigCommerce.API.Models;
using Avrhil.BigCommerce.API.Services;

namespace Avrhil.BigCommerce.API;

class Program
{
    static async Task Main(string[] args)
    {
        // OAuth Configuration
        var clientId = "1m4pmh2zfs4hc8w2xsu16ghnnmiekzc";
        var clientSecret = "a08501208fbb8030078097fea0da7eb5547bb57fc08fa6af5a69ca5428d73ec1";
        var AccountUUID = "072f7c4e-066f-4887-bd8a-590a38fb718d";
        var redirectUri = "http://localhost/oauth";

        var oauthService = new BigCommerceOAuthService(clientId, clientSecret, redirectUri);

        // Step 1: Get authorization URL
        var authUrl = oauthService.GetAuthorizationUrl();
        Console.WriteLine("Visit this URL to authorize:");
        Console.WriteLine(authUrl);
        Console.WriteLine();

        // Step 2: After user authorizes, you'll receive code, context, and scope in callback
        Console.WriteLine("Enter the authorization code:");
        var code = Console.ReadLine();
        
        Console.WriteLine("Enter the context (store hash):");
        var context = Console.ReadLine();
        
        Console.WriteLine("Enter the scope:");
        var scope = Console.ReadLine();

        try
        {
            // Step 3: Exchange code for access token
            var tokenResponse = await oauthService.ExchangeCodeForTokenAsync(code, context, scope);
            Console.WriteLine($"Access Token: {tokenResponse.access_token}");
            Console.WriteLine($"Store Hash: {tokenResponse.context}");
            Console.WriteLine();

            // Step 4: Use the access token to make API calls
            var storeHash = tokenResponse.context.Replace("stores/", "");
            var apiClient = new BigCommerceApiClient(storeHash, tokenResponse.access_token);

            Console.WriteLine("Fetching products...");
            var products = await apiClient.GetAsync<ProductsResponse>("catalog/products");
            Console.WriteLine($"Total products: {products.Data.Count}");
            Console.WriteLine("API calls completed successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
