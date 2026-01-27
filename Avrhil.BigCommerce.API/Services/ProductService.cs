using Avrhil.BigCommerce.API.Models;
using System.Data;

namespace Avrhil.BigCommerce.API.Services;

public class ProductService
{
    private readonly BigCommerceApiClient _apiClient;

    public ProductService(BigCommerceApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<List<Product>> FetchAllProductsAsync()
    {
        var products = await _apiClient.GetAsync<ProductsResponse>("catalog/products");
        return products.Data;
    }

    public void CacheProductsToCsv(List<Product> products, string filePath)
    {
        var dataTable = CsvHandler.ConvertToDataTable(products);
        CsvHandler.WriteCsv(dataTable, filePath);
    }

    public async Task<List<ProductResponse>> BulkInsertProductsFromCsvAsync(string csvFilePath, int defaultCategoryId = 0)
    {
        var dataTable = CsvHandler.ReadCsv(csvFilePath);
        var responses = new List<ProductResponse>();

        foreach (DataRow row in dataTable.Rows)
        {
            var productRequest = new ProductCreateRequest
            {
                Name = row["Product Name"].ToString(),
                Sku = row["SKU"].ToString(),
                Type = row["Type"].ToString(),
                Price = decimal.Parse(row["Price"].ToString()),
                Weight = decimal.Parse(row["Weight"].ToString()),
                Description = row["Description"].ToString(),
                InventoryLevel = int.Parse(row["Inventory Level"].ToString()),
                Availability = row["Availability"].ToString(),
                Categories = defaultCategoryId > 0 ? new List<int> { defaultCategoryId } : new List<int>()
            };

            try
            {
                var response = await _apiClient.PostAsync<ProductResponse>("catalog/products", productRequest);
                responses.Add(response);
                Console.WriteLine($"✓ Inserted: {productRequest.Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Failed: {productRequest.Name} - {ex.Message}");
            }
        }

        return responses;
    }

    public async Task<bool> UpdateInventoryAsync(int productId, int inventoryLevel)
    {
        var payload = new { inventory_level = inventoryLevel };
        await _apiClient.PutAsync<ProductResponse>($"catalog/products/{productId}", payload);
        return true;
    }

    public async Task<bool> UpdateInventoryBySku(string sku, int inventoryLevel)
    {
        var products = await _apiClient.GetAsync<ProductsResponse>($"catalog/products?sku={sku}");
        if (products.Data.Count > 0)
        {
            return await UpdateInventoryAsync(products.Data[0].Id, inventoryLevel);
        }
        return false;
    }
}
