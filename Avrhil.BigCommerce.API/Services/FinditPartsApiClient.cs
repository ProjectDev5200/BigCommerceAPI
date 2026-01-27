using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Avrhil.BigCommerce.API.Models;

namespace Avrhil.BigCommerce.API.Services;

public class FinditPartsApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _baseUrl = "https://api.finditparts.com/v1";

    public FinditPartsApiClient(string apiKey)
    {
        _apiKey = apiKey;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("X-API-Key", _apiKey);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<PartSearchResponse> SearchPartsAsync(PartSearchRequest request)
    {
        var queryParams = new List<string>();
        if (!string.IsNullOrEmpty(request.PartNumber)) queryParams.Add($"partNumber={Uri.EscapeDataString(request.PartNumber)}");
        if (!string.IsNullOrEmpty(request.Brand)) queryParams.Add($"brand={Uri.EscapeDataString(request.Brand)}");
        if (request.Year.HasValue) queryParams.Add($"year={request.Year}");
        if (!string.IsNullOrEmpty(request.Make)) queryParams.Add($"make={Uri.EscapeDataString(request.Make)}");
        if (!string.IsNullOrEmpty(request.Model)) queryParams.Add($"model={Uri.EscapeDataString(request.Model)}");
        
        var query = string.Join("&", queryParams);
        return await GetAsync<PartSearchResponse>($"parts/search?{query}");
    }

    public async Task<Part> GetPartByIdAsync(string partId)
    {
        return await GetAsync<Part>($"parts/{partId}");
    }

    public async Task<VehicleResponse> GetVehiclesByYearAsync(int year)
    {
        return await GetAsync<VehicleResponse>($"vehicles?year={year}");
    }

    public async Task<VehicleResponse> GetVehiclesByYearMakeAsync(int year, string make)
    {
        return await GetAsync<VehicleResponse>($"vehicles?year={year}&make={Uri.EscapeDataString(make)}");
    }

    public async Task<OrderResponse> CreateOrderAsync(OrderRequest order)
    {
        return await PostAsync<OrderResponse>("orders", order);
    }

    public async Task<OrderResponse> GetOrderStatusAsync(string orderId)
    {
        return await GetAsync<OrderResponse>($"orders/{orderId}");
    }

    public async Task<decimal> GetPartPriceAsync(string partNumber)
    {
        var response = await GetAsync<Part>($"parts/{partNumber}/price");
        return response.Price;
    }

    public async Task<int> GetPartInventoryAsync(string partNumber)
    {
        var response = await GetAsync<Part>($"parts/{partNumber}/inventory");
        return response.Quantity;
    }

    private async Task<T> GetAsync<T>(string endpoint)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/{endpoint}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(content);
    }

    private async Task<T> PostAsync<T>(string endpoint, object data)
    {
        var json = JsonConvert.SerializeObject(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"{_baseUrl}/{endpoint}", content);
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(responseContent);
    }
}
