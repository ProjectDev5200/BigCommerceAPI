using System.Text.Json.Serialization;

namespace FindItParts.Searchparts.Models;

public class FindItPartsProduct
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("part_number")]
    public string PartNumber { get; set; }
    
    [JsonPropertyName("short_description")]
    public string Description { get; set; }
    
    [JsonPropertyName("manufacturer")]
    public string Manufacturer { get; set; }
    
    [JsonPropertyName("brand")]
    public string Brand { get; set; }
    
    public decimal CorePrice { get; set; }
    public decimal AccountPrice { get; set; }
    
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
    
    [JsonPropertyName("inventory_status")]
    public string InventoryStatus { get; set; }
    
    [JsonPropertyName("image")]
    public string ImageUrl { get; set; }
    
    [JsonPropertyName("tags")]
    public List<string> Tags { get; set; }
    
    [JsonPropertyName("pies_attributes")]
    public PiesAttributes PiesAttributes { get; set; }
    
    [JsonPropertyName("variants")]
    public List<ProductVariant> Variants { get; set; }
}

public class ProductVariant
{
    [JsonPropertyName("variant_id")]
    public string VariantId { get; set; }
    
    [JsonPropertyName("price")]
    public decimal Price { get; set; }
    
    [JsonPropertyName("account_price")]
    public decimal AccountPrice { get; set; }
    
    [JsonPropertyName("core_price")]
    public decimal CorePrice { get; set; }
    
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
    
    [JsonPropertyName("location")]
    public string Location { get; set; }
    
    [JsonPropertyName("estimated_business_days_to_ship")]
    public int EstimatedShipDays { get; set; }
}

public class PiesAttributes
{
    [JsonPropertyName("part_terminology_id")]
    public string PartTerminologyId { get; set; }
    
    [JsonPropertyName("part_type")]
    public string PartType { get; set; }
    
    [JsonPropertyName("position")]
    public string Position { get; set; }
    
    [JsonPropertyName("weight")]
    public string Weight { get; set; }
    
    [JsonPropertyName("dimensions")]
    public string Dimensions { get; set; }
    
    [JsonPropertyName("material")]
    public string Material { get; set; }
    
    [JsonPropertyName("country_of_origin")]
    public string CountryOfOrigin { get; set; }
}

public class SearchResponse
{
    public List<FindItPartsProduct> Products { get; set; }
    public int TotalResults { get; set; }
}
