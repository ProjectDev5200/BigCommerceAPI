using Avrhil.BigCommerce.API.Models;

namespace Avrhil.BigCommerce.API.Services;

public class CatalogService
{
    private readonly BigCommerceApiClient _apiClient;

    public CatalogService(BigCommerceApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    // Product Variants
    public async Task<ProductVariantsResponse> GetProductVariantsAsync(int productId)
    {
        return await _apiClient.GetAsync<ProductVariantsResponse>($"catalog/products/{productId}/variants");
    }

    public async Task<ProductVariantResponse> GetProductVariantAsync(int productId, int variantId)
    {
        return await _apiClient.GetAsync<ProductVariantResponse>($"catalog/products/{productId}/variants/{variantId}");
    }

    public async Task<ProductVariantResponse> CreateProductVariantAsync(int productId, ProductVariant variant)
    {
        return await _apiClient.PostAsync<ProductVariantResponse>($"catalog/products/{productId}/variants", variant);
    }

    public async Task<ProductVariantResponse> UpdateProductVariantAsync(int productId, int variantId, ProductVariant variant)
    {
        return await _apiClient.PutAsync<ProductVariantResponse>($"catalog/products/{productId}/variants/{variantId}", variant);
    }

    public async Task<bool> DeleteProductVariantAsync(int productId, int variantId)
    {
        return await _apiClient.DeleteAsync($"catalog/products/{productId}/variants/{variantId}");
    }

    // Product Modifiers
    public async Task<ProductModifiersResponse> GetProductModifiersAsync(int productId)
    {
        return await _apiClient.GetAsync<ProductModifiersResponse>($"catalog/products/{productId}/modifiers");
    }

    public async Task<ProductModifierResponse> CreateProductModifierAsync(int productId, ProductModifier modifier)
    {
        return await _apiClient.PostAsync<ProductModifierResponse>($"catalog/products/{productId}/modifiers", modifier);
    }

    public async Task<bool> DeleteProductModifierAsync(int productId, int modifierId)
    {
        return await _apiClient.DeleteAsync($"catalog/products/{productId}/modifiers/{modifierId}");
    }

    // Product Options
    public async Task<ProductOptionsResponse> GetProductOptionsAsync(int productId)
    {
        return await _apiClient.GetAsync<ProductOptionsResponse>($"catalog/products/{productId}/options");
    }

    public async Task<ProductOptionResponse> CreateProductOptionAsync(int productId, ProductOption option)
    {
        return await _apiClient.PostAsync<ProductOptionResponse>($"catalog/products/{productId}/options", option);
    }

    public async Task<bool> DeleteProductOptionAsync(int productId, int optionId)
    {
        return await _apiClient.DeleteAsync($"catalog/products/{productId}/options/{optionId}");
    }

    // Brands
    public async Task<BrandsResponse> GetBrandsAsync()
    {
        return await _apiClient.GetAsync<BrandsResponse>("catalog/brands");
    }

    public async Task<BrandResponse> GetBrandAsync(int brandId)
    {
        return await _apiClient.GetAsync<BrandResponse>($"catalog/brands/{brandId}");
    }

    public async Task<BrandResponse> CreateBrandAsync(Brand brand)
    {
        return await _apiClient.PostAsync<BrandResponse>("catalog/brands", brand);
    }

    public async Task<BrandResponse> UpdateBrandAsync(int brandId, Brand brand)
    {
        return await _apiClient.PutAsync<BrandResponse>($"catalog/brands/{brandId}", brand);
    }

    public async Task<bool> DeleteBrandAsync(int brandId)
    {
        return await _apiClient.DeleteAsync($"catalog/brands/{brandId}");
    }

    // Categories
    public async Task<CategoriesResponse> GetCategoriesAsync()
    {
        return await _apiClient.GetAsync<CategoriesResponse>("catalog/categories");
    }

    public async Task<CategoryResponse> GetCategoryAsync(int categoryId)
    {
        return await _apiClient.GetAsync<CategoryResponse>($"catalog/categories/{categoryId}");
    }

    public async Task<CategoryResponse> CreateCategoryAsync(Category category)
    {
        return await _apiClient.PostAsync<CategoryResponse>("catalog/categories", category);
    }

    public async Task<CategoryResponse> UpdateCategoryAsync(int categoryId, Category category)
    {
        return await _apiClient.PutAsync<CategoryResponse>($"catalog/categories/{categoryId}", category);
    }

    public async Task<bool> DeleteCategoryAsync(int categoryId)
    {
        return await _apiClient.DeleteAsync($"catalog/categories/{categoryId}");
    }
}
