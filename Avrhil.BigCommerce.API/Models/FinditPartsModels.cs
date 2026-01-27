namespace Avrhil.BigCommerce.API.Models;

public class PartSearchRequest
{
    public string PartNumber { get; set; }
    public string Brand { get; set; }
    public int? Year { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
}

public class PartSearchResponse
{
    public List<Part> Parts { get; set; }
    public int TotalResults { get; set; }
}

public class Part
{
    public string PartNumber { get; set; }
    public string Brand { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string ImageUrl { get; set; }
}

public class VehicleInfo
{
    public int Year { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public string Engine { get; set; }
}

public class VehicleResponse
{
    public List<VehicleInfo> Vehicles { get; set; }
}

public class OrderRequest
{
    public string OrderNumber { get; set; }
    public List<OrderItem> Items { get; set; }
    public ShippingAddress ShippingAddress { get; set; }
}

public class OrderItem
{
    public string PartNumber { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

public class ShippingAddress
{
    public string Name { get; set; }
    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string Country { get; set; }
}

public class FindItpartsOrderResponse
{
    public string OrderId { get; set; }
    public string Status { get; set; }
    public string TrackingNumber { get; set; }
}
