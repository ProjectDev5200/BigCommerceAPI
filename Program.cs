using Avrhil.BigCommerce.API.Models;
using Avrhil.BigCommerce.API.Services;

namespace Avrhil.BigCommerce.API;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            string clientid = "9dfvw7w2hdcaa2f4p258ur0jng1aimq";
            string clientsecret = "b867a0c92ec18a0284c7e66eb21c3780ff9fb97fcca15c67ac9755b49cf939c4";
            // Step 3: Exchange code for access token
            // Step 4: Use the access token to make API calls
            Console.WriteLine("Enter the context (store hash):");
            var context = Console.ReadLine();

            var storeHash = context;// tokenResponse.context.Replace("stores/", "");
            string access_token = "726idlafywsm5bim5pisza0zhja0w5a";

            Console.WriteLine("Enter the store hash:");
            
            Console.WriteLine("Enter the access token:");
           // var accessToken = Console.ReadLine();
            
            var apiClient = new BigCommerceApiClient(storeHash, access_token);
            var productService = new ProductService(apiClient);

            Console.WriteLine("\n=== BigCommerce Product Integration ===");
            Console.WriteLine("1. Fetch products and cache to CSV");
            Console.WriteLine("2. Insert sample products from CSV");
            Console.WriteLine("3. Both (Fetch then Insert)");
            Console.Write("Select option: ");
            var option = Console.ReadLine();

            if (option == "1" || option == "3")
            {
                Console.WriteLine("\nFetching products from BigCommerce...");
                var products = await productService.FetchAllProductsAsync();
                Console.WriteLine($"Fetched {products.Count} products");
                
                var cacheFile = "bigcommerce_products_cache.csv";
                productService.CacheProductsToCsv(products, cacheFile);
                Console.WriteLine($"✓ Products cached to {cacheFile}");
            }

            if (option == "2" || option == "3")
            {
                Console.WriteLine("\nInserting sample products...");
                var responses = await productService.BulkInsertProductsFromCsvAsync("sample_products.csv");
                Console.WriteLine($"\n✓ Inserted {responses.Count} products successfully");
            }

            Console.WriteLine("\nCompleted!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
