using FindItParts.Searchparts.Models;
using System.Text.Json;

namespace FindItParts.Searchparts.Services;

public class FindItPartsApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiBaseUrl;
    private readonly string _apiKey;

    public FindItPartsApiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiBaseUrl = configuration["FindItPartsApi:BaseUrl"] ?? "https://finditparts.com/api/v1";
        _apiKey = configuration["FindItPartsApi:ApiKey"];
        
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        if (!string.IsNullOrEmpty(_apiKey))
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer api-"+_apiKey);
        }


    }

    public async Task<SearchResponse> SearchProductsAsync(string query, int page = 1, int per = 20)
    {
        var url = $"{_apiBaseUrl}/products?query={Uri.EscapeDataString(query)}&page={page}&per={per}";
        
        try
        {
            var response = await _httpClient.GetAsync(url);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<FindItPartsApiResponse>(content, new JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true 
                });
                
                return new SearchResponse 
                { 
                    Products = apiResponse?.Products ?? new List<FindItPartsProduct>(),
                    TotalResults = apiResponse?.Total ?? 0
                };
            }
            else
            {
                var result= response.Content.ReadAsStringAsync().Result;
            }
        }
        catch (Exception)
        {
            // Log error if needed
        }
        
        return new SearchResponse { Products = new List<FindItPartsProduct>(), TotalResults = 0 };
    }


}

public class FindItPartsApiResponse
{
    public List<FindItPartsProduct> Products { get; set; }
    public int Total { get; set; }
}
