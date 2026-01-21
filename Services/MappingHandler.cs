using Avrhil.BigCommerce.API.Models;

namespace Avrhil.BigCommerce.API.Services;

public class MappingHandler
{
    private readonly BigCommerceApiClient _bigCommerceClient;
    private readonly FinditPartsApiClient _finditPartsClient;

    public MappingHandler(BigCommerceApiClient bigCommerceClient, FinditPartsApiClient finditPartsClient)
    {
        _bigCommerceClient = bigCommerceClient;
        _finditPartsClient = finditPartsClient;
    }

    public async Task<Product> ImportPartAsProductAsync(string partNumber)
    {
        var part = await _finditPartsClient.GetPartByIdAsync(partNumber);
        var product = ProductMapper.ToProduct(part);
        return await _bigCommerceClient.PostAsync<Product>("catalog/products", product);
    }

    public async Task<List<Product>> ImportPartsAsProductsAsync(PartSearchRequest searchRequest)
    {
        var searchResponse = await _finditPartsClient.SearchPartsAsync(searchRequest);
        var products = ProductMapper.ToProducts(searchResponse.Parts);
        
        var importedProducts = new List<Product>();
        foreach (var product in products)
        {
            var imported = await _bigCommerceClient.PostAsync<Product>("catalog/products", product);
            importedProducts.Add(imported);
        }
        return importedProducts;
    }

    public async Task SyncInventoryAsync(string sku)
    {
        var quantity = await _finditPartsClient.GetPartInventoryAsync(sku);
        await _bigCommerceClient.PutAsync<object>($"catalog/products/{sku}/inventory", new { Quantity = quantity });
    }

    public async Task SyncPriceAsync(string sku)
    {
        var price = await _finditPartsClient.GetPartPriceAsync(sku);
        await _bigCommerceClient.PutAsync<object>($"catalog/products/{sku}", new { Price = price });
    }
}
