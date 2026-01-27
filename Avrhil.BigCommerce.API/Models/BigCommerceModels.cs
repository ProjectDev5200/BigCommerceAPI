using Newtonsoft.Json;

namespace Avrhil.BigCommerce.API.Models;

public class Product
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("type")]
    public string Type { get; set; }
    
    [JsonProperty("sku")]
    public string Sku { get; set; }
    
    [JsonProperty("price")]
    public decimal Price { get; set; }
    
    [JsonProperty("weight")]
    public decimal Weight { get; set; }
    
    [JsonProperty("description")]
    public string Description { get; set; }
    
    [JsonProperty("categories")]
    public List<int> Categories { get; set; }
    
    [JsonProperty("inventory_level")]
    public int? InventoryLevel { get; set; }
    
    [JsonProperty("availability")]
    public string Availability { get; set; }
}

public class ProductCreateRequest
{
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("type")]
    public string Type { get; set; }
    
    [JsonProperty("sku")]
    public string Sku { get; set; }
    
    [JsonProperty("price")]
    public decimal Price { get; set; }
    
    [JsonProperty("weight")]
    public decimal Weight { get; set; }
    
    [JsonProperty("description")]
    public string Description { get; set; }
    
    [JsonProperty("categories")]
    public List<int> Categories { get; set; }
    
    [JsonProperty("inventory_level")]
    public int InventoryLevel { get; set; }
    
    [JsonProperty("availability")]
    public string Availability { get; set; }
}

public class ProductResponse
{
    public Product Data { get; set; }
    public Meta Meta { get; set; }
}

public class ProductsResponse
{
    public List<Product> Data { get; set; }
    public Meta Meta { get; set; }
}

public class Meta
{
    public Pagination Pagination { get; set; }
}

public class Pagination
{
    public int Total { get; set; }
    public int Count { get; set; }
    public int PerPage { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}

public class Order
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("status_id")]
    public int StatusId { get; set; }
    
    [JsonProperty("customer_id")]
    public int CustomerId { get; set; }
    
    [JsonProperty("date_created")]
    public string DateCreated { get; set; }
    
    [JsonProperty("total_inc_tax")]
    public decimal TotalIncTax { get; set; }
    
    [JsonProperty("billing_address")]
    public Address BillingAddress { get; set; }
    
    [JsonProperty("shipping_addresses")]
    public List<Address> ShippingAddresses { get; set; }
}

public class Address
{
    [JsonProperty("first_name")]
    public string FirstName { get; set; }
    
    [JsonProperty("last_name")]
    public string LastName { get; set; }
    
    [JsonProperty("street_1")]
    public string Street1 { get; set; }
    
    [JsonProperty("city")]
    public string City { get; set; }
    
    [JsonProperty("state")]
    public string State { get; set; }
    
    [JsonProperty("zip")]
    public string Zip { get; set; }
    
    [JsonProperty("country")]
    public string Country { get; set; }
}

public class OrderShipment
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("order_id")]
    public int OrderId { get; set; }
    
    [JsonProperty("tracking_number")]
    public string TrackingNumber { get; set; }
    
    [JsonProperty("shipping_method")]
    public string ShippingMethod { get; set; }
    
    [JsonProperty("items")]
    public List<ShipmentItem> Items { get; set; }
}

public class ShipmentItem
{
    [JsonProperty("order_product_id")]
    public int OrderProductId { get; set; }
    
    [JsonProperty("quantity")]
    public int Quantity { get; set; }
}

public class OrderShipmentRequest
{
    [JsonProperty("tracking_number")]
    public string TrackingNumber { get; set; }
    
    [JsonProperty("shipping_method")]
    public string ShippingMethod { get; set; }
    
    [JsonProperty("items")]
    public List<ShipmentItem> Items { get; set; }
}

public class OrderResponse
{
    public Order Data { get; set; }
}

public class OrdersResponse
{
    public List<Order> Data { get; set; }
    public Meta Meta { get; set; }
}

public class OrderShipmentResponse
{
    public OrderShipment Data { get; set; }
}

public class OrderShipmentsResponse
{
    public List<OrderShipment> Data { get; set; }
}

public class ProductVariant
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("product_id")]
    public int ProductId { get; set; }
    
    [JsonProperty("sku")]
    public string Sku { get; set; }
    
    [JsonProperty("price")]
    public decimal? Price { get; set; }
    
    [JsonProperty("inventory_level")]
    public int InventoryLevel { get; set; }
    
    [JsonProperty("option_values")]
    public List<VariantOptionValue> OptionValues { get; set; }
}

public class VariantOptionValue
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("option_id")]
    public int OptionId { get; set; }
    
    [JsonProperty("label")]
    public string Label { get; set; }
}

public class ProductModifier
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("product_id")]
    public int ProductId { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("display_name")]
    public string DisplayName { get; set; }
    
    [JsonProperty("type")]
    public string Type { get; set; }
    
    [JsonProperty("required")]
    public bool Required { get; set; }
}

public class ProductOption
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("product_id")]
    public int ProductId { get; set; }
    
    [JsonProperty("display_name")]
    public string DisplayName { get; set; }
    
    [JsonProperty("type")]
    public string Type { get; set; }
    
    [JsonProperty("option_values")]
    public List<OptionValue> OptionValues { get; set; }
}

public class OptionValue
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("label")]
    public string Label { get; set; }
    
    [JsonProperty("sort_order")]
    public int SortOrder { get; set; }
}

public class Brand
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("page_title")]
    public string PageTitle { get; set; }
}

public class Category
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("parent_id")]
    public int ParentId { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("description")]
    public string Description { get; set; }
    
    [JsonProperty("is_visible")]
    public bool IsVisible { get; set; }
}

public class ProductVariantsResponse
{
    public List<ProductVariant> Data { get; set; }
    public Meta Meta { get; set; }
}

public class ProductVariantResponse
{
    public ProductVariant Data { get; set; }
}

public class ProductModifiersResponse
{
    public List<ProductModifier> Data { get; set; }
}

public class ProductModifierResponse
{
    public ProductModifier Data { get; set; }
}

public class ProductOptionsResponse
{
    public List<ProductOption> Data { get; set; }
}

public class ProductOptionResponse
{
    public ProductOption Data { get; set; }
}

public class BrandsResponse
{
    public List<Brand> Data { get; set; }
    public Meta Meta { get; set; }
}

public class BrandResponse
{
    public Brand Data { get; set; }
}

public class CategoriesResponse
{
    public List<Category> Data { get; set; }
    public Meta Meta { get; set; }
}

public class CategoryResponse
{
    public Category Data { get; set; }
}
